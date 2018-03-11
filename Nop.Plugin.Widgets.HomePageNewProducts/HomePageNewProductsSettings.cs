using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsSettings : ISettings
    {
        public int ProductsCount { get; set; }

        public string SelectedZone { get; set; }
    }
}
