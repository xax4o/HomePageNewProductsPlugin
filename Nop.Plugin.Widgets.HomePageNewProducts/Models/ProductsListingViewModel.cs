using Nop.Core.Domain.Catalog;
using System;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Models
{
    public class ProductsListingViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SeName { get; set; }

        public string PictureUrl { get; set; }

        public string Price { get; set; }

        public string CreatedOn { get; set; }
    }
}
