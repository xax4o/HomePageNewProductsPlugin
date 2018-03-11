using System.Linq;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToFriendlyText(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            input = input.Replace('_', ' ');
            input = input.First().ToString().ToUpper() + input.Substring(1);

            return input;
        }
    }
}
