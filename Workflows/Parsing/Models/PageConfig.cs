using Newtonsoft.Json;

namespace K2Warmup.Workflows.Parsing.Models
{
    public class PageConfig
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("waitForElement")]
        public bool WaitForElement { get; set; }

        [JsonProperty("elementSelector")]
        public string ElementSelector { get; set; }

        [JsonProperty("selectorType")]
        public string SelectorType { get; set; }

        [JsonProperty("timeoutOverride")]
        public int? TimeoutOverride { get; set; }
    }
}