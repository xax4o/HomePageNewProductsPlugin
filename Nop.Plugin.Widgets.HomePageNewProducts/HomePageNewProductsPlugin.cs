using Nop.Core;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.HomePageNewProducts.Common;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsPlugin : BasePlugin, IWidgetPlugin
    {
        private const int DefaultShownProductsCount = 4;

        private const string LatestProductsLacaleDefaultValue = "Latest products";
        private const string ProductsCountDefaultLacaleValue = "Select Products Count";
        private const string SelectZoneDefaultLacaleValue = "Select Zone";
        private const string DateAddedDefaultLacaleValue = "Added: {0}";

        private const string PathToConfigure = "Admin/WidgetsHomePageNewProducts/Configure";

        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public HomePageNewProductsPlugin(ISettingService settingService, IWebHelper webHelper)
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return HomePageZonesGenerator.GetList();
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + PathToConfigure;
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return LocalConstants.WidgetViewComponentName;
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new HomePageNewProductsSettings
            {
                ProductsCount = DefaultShownProductsCount,
                SelectedZone = PublicWidgetZones.HomePageTop
            };

            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource(LocalConstants.LatestProductsLacaleKey, LatestProductsLacaleDefaultValue);
            this.AddOrUpdatePluginLocaleResource(LocalConstants.ProductsCountLocaleKey, ProductsCountDefaultLacaleValue);
            this.AddOrUpdatePluginLocaleResource(LocalConstants.SelectZoneLocaleKey, SelectZoneDefaultLacaleValue);
            this.AddOrUpdatePluginLocaleResource(LocalConstants.AddedOnLocaleKey, DateAddedDefaultLacaleValue);

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<HomePageNewProductsSettings>();

            this.DeletePluginLocaleResource(LocalConstants.LatestProductsLacaleKey);
            this.DeletePluginLocaleResource(LocalConstants.ProductsCountLocaleKey);
            this.DeletePluginLocaleResource(LocalConstants.SelectZoneLocaleKey);
            this.DeletePluginLocaleResource(LocalConstants.AddedOnLocaleKey);

            base.Uninstall();
        }
    }
}