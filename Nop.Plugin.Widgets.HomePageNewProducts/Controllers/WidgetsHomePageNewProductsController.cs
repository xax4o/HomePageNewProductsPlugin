using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.HomePageNewProducts.Common;
using Nop.Plugin.Widgets.HomePageNewProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AdminAntiForgery]
    public class WidgetsHomePageNewProductsController : BasePluginController
    {
        private const string ConfigureViewPath = "~/Plugins/Widgets.HomePageNewProducts/Views/Configure.cshtml";
        private const string SuccessSaveMessageKey = "Admin.Plugins.Saved";

        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        public WidgetsHomePageNewProductsController(IWorkContext workContext,
            IStoreService storeService,
            IPermissionService permissionService,
            ISettingService settingService,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._localizationService = localizationService;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
            {
                return AccessDeniedView();
            }

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);

            if (homePageNewProductsSettings == null)
            {
                throw new NullReferenceException(nameof(homePageNewProductsSettings));
            }

            var model = new ConfigurationModel()
            {
                ProductsCount = homePageNewProductsSettings.ProductsCount,
                // Not sure but the options may need to be localized to. For now they aren't.
                HomePageZones = HomePageZonesGenerator.GetSelectList(homePageNewProductsSettings.SelectedZone)
            };

            return View(ConfigureViewPath, model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
            {
                return AccessDeniedView();
            }

            if (!ModelState.IsValid)
            {
                return Configure();
            }

            var newSetting = new HomePageNewProductsSettings()
            {
                ProductsCount = model.ProductsCount,
                SelectedZone = model.SelectedHomePageZone
            };

            _settingService.SaveSetting(newSetting);

            SuccessNotification(_localizationService.GetResource(SuccessSaveMessageKey));

            return Configure();
        }
    }
}
