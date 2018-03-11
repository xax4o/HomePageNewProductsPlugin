using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.HomePageNewProducts.Common;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Models
{
    public class ConfigurationModel
    {
        //I've searched but did not find how to set localization for the error messages without injecting LocalizationService.
        [Range(LocalConstants.MinProductsCount, LocalConstants.MaxProductsCount)]
        [NopResourceDisplayName(LocalConstants.ProductsCountLocaleKey)]
        public int ProductsCount { get; set; }

        public IEnumerable<SelectListItem> HomePageZones { get; set; }

        [Required]
        [NopResourceDisplayName(LocalConstants.SelectZoneLocaleKey)]
        public string SelectedHomePageZone { get; set; }
    }
}
