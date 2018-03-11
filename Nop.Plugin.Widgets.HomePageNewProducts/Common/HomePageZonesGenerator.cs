using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.HomePageNewProducts.Common.Extensions;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Common
{
    public static class HomePageZonesGenerator
    {
        public static IList<string> GetList()
        {
            return new List<string>
            {
                PublicWidgetZones.HomePageTop,
                PublicWidgetZones.HomePageBeforeCategories,
                PublicWidgetZones.HomePageBeforeProducts,
                PublicWidgetZones.HomePageBeforeBestSellers,
                PublicWidgetZones.HomePageBeforeNews,
                PublicWidgetZones.HomePageBeforePoll,
                PublicWidgetZones.HomePageBottom
            };
        }

        public static IList<SelectListItem> GetSelectList(string selectedItem = "")
        {
            var zones = GetList();
            var selectList = new List<SelectListItem>();

            foreach (var zone in zones)
            {
                var selectListItem = new SelectListItem()
                {
                    Value = zone,
                    Text = zone.ToFriendlyText()
                };

                if (!string.IsNullOrWhiteSpace(selectedItem) && selectedItem == zone)
                {
                    selectListItem.Selected = true;
                }

                selectList.Add(selectListItem);
            }

            return selectList;
        }
    }
}
