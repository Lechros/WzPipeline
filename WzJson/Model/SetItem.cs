using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class SetItem
{
    public string? Name { get; set; }

    public List<int> ItemIds { get; set; } = new();

    public SortedDictionary<int, Dictionary<string, int>> Effects { get; set; } = new();

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool JokerPossible { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ZeroWeaponJokerPossible { get; set; }

    public bool ShouldSerializeItems() => ItemIds.Count > 0;
    public bool ShouldSerializeEffect() => Effects.Count > 0;
}