using System;
using System.Collections.Generic;
using System.Data;
using UiPath.Activities.System.Jobs.Coded;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebApplicationWarmup.Workflows.Parsing.Models;

namespace WebApplicationWarmup.Workflows.Parsing.Services
{
public class PageConfigs : IEnumerable<PageConfig>
{
    private readonly List<PageConfig> _items;

    public PageConfigs(string json)
    {
        _items = JsonConvert.DeserializeObject<List<PageConfig>>(json)
            ?? new List<PageConfig>();
    }

    public IEnumerator<PageConfig> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
}