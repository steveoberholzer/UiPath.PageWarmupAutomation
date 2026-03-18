using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using UiPath.Activities.System.Jobs.Coded;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;

namespace WebApplicationWarmup.Workflows.Parsing.Models
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