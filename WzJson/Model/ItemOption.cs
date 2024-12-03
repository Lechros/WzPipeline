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

    public SortedDictionary<int, LevelInfo> Level { get; set; } = new();

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class LevelInfo
    {
        public required string String { get; set; }

        public GearOption Option { get; set; } = new();
    }
}