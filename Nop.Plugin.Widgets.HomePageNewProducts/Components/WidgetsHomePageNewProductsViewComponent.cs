using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Plugin.Widgets.HomePageNewProducts.Common;
using Nop.Plugin.Widgets.HomePageNewProducts.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;
using System;
using System.Linq;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Components
{
    [ViewComponent(Name = LocalConstants.WidgetViewComponentName)]
    public class WidgetsHomePageNewProductsViewComponent : NopViewComponent
    {
        private const string PictureUrlModelCacheKey = "Nop.plugins.widgets.homepagenewproducts.pictureurl.for.product-{0}";
        private const string PublicInfoViewPath = "~/Plugins/Widgets.HomePageNewProducts/Views/PublicInfo.cshtml";

        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICurrencyService _currencyService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly IDateTimeHelper _dateTimeHelper;

        public WidgetsHomePageNewProductsViewComponent(
            IStoreContext storeContext,
            ISettingService settingService,
            IProductService productService,
            IPriceFormatter priceFormatter,
            ICurrencyService currencyService,
            IWorkContext workContext,
            ICacheManager cacheManager,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            IDateTimeHelper dateTimeHelper)
        {
            this._storeContext = storeContext;
            this._settingService = settingService;
            this._productService = productService;
            this._priceFormatter = priceFormatter;
            this._currencyService = currencyService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
            this._dateTimeHelper = dateTimeHelper;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(_storeContext.CurrentStore.Id);

            if (homePageNewProductsSettings == null)
            {
                throw new NullReferenceException(nameof(homePageNewProductsSettings));
            }

            if (homePageNewProductsSettings.SelectedZone != widgetZone)
            {
                return Content("");
            }

            const int pageIndex = 0;

            var model = _productService
                .SearchProducts(pageIndex: pageIndex, pageSize: homePageNewProductsSettings.ProductsCount, searchManufacturerPartNumber: false, searchSku: false, orderBy: ProductSortingEnum.CreatedOn)
                .Select(p => new ProductsListingViewModel
                {
                    Id = p.Id,
                    Name = p.GetLocalized(pr => pr.Name),
                    PictureUrl = GetPictureUrl(p),
                    SeName = p.GetSeName(),
                    Price = GetPriceFormatted(p.Price),
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(p.CreatedOnUtc).ToString()
                });

            return View(PublicInfoViewPath, model);
        }

        private string GetPriceFormatted(decimal price)
        {
            // Not sure what to do with the discounts and old prices stuff.
            var priceInCurrentCurrency = _currencyService.ConvertFromPrimaryStoreCurrency(price, _workContext.WorkingCurrency);
            var priceFoFormattedString = _priceFormatter.FormatPrice(priceInCurrentCurrency);

            return priceFoFormattedString;
        }

        private string GetPictureUrl(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var cacheKey = string.Format(PictureUrlModelCacheKey, product.Id);

            return _cacheManager.Get(cacheKey, () =>
            {
                var picture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                return _pictureService.GetPictureUrl(picture, _mediaSettings.ProductThumbPictureSize);
            });
        }
    }
}