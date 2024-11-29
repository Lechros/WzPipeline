using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ItemOption
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int OptionType { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ReqLevel { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? String { get; set; }

    public SortedDictionary<int, Dictionary<string, object>> Level { get; set; } = new();
    
    public bool ShouldSerializeLevel() => Level.Count > 0;
}