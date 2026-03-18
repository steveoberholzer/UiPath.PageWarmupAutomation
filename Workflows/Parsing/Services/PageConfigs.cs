using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using K2Warmup.Workflows.Parsing.Models;

namespace K2Warmup.Workflows.Parsing.Services
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