using System;

namespace K2Warmup.Workflows.Helpers
{
    public class Selectors
    {
        /// <summary>
        /// Translates a CSS selector string into a UiPath web selector string.
        /// </summary>
        /// <param name="cssSelector">The CSS selector, e.g. "body > div.L3eUgb > h1"</param>
        /// <returns>A UiPath-compatible selector string, e.g. "&lt;webctrl css-selector='body&gt;div.L3eUgb&gt;h1' /&gt;"</returns>
        public static string CssSelectorToUiPathSelector(string cssSelector)
        {
            if (string.IsNullOrWhiteSpace(cssSelector))
                throw new ArgumentException("CSS selector cannot be null or empty.", nameof(cssSelector));

            // Normalise whitespace around combinators, then strip remaining whitespace
            var normalised = cssSelector
                .Trim()
                .Replace(" > ", ">")   // descendant combinator with spaces
                .Replace(" + ", "+")   // adjacent sibling
                .Replace(" ~ ", "~")   // general sibling
                .Replace("  ", " ");   // collapse any double spaces

            // XML-encode characters that are meaningful inside an XML attribute value
            var encoded = normalised
                .Replace("&", "&amp;")   // must be first
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");

            return $"<webctrl css-selector='{encoded}' />";
        }
    }
}