using Newtonsoft.Json;

namespace WzJson.Model;

public class SetItem
{
    [JsonProperty(Order = 1, PropertyName = "name")]
    public string? Name { get; set; }

    [JsonProperty(Order = 2, PropertyName = "itemIds")]
    public List<int> ItemIds { get; set; } = new();

    [JsonProperty(Order = 3, PropertyName = "effects")]
    public SortedDictionary<int, Dictionary<string, int>> Effects { get; set; } = new();

    [JsonProperty(Order = 4, PropertyName = "jokerPossible")]
    public bool JokerPossible { get; set; }

    [JsonProperty(Order = 5, PropertyName = "zeroWeaponJokerPossible")]
    public bool ZeroWeaponJokerPossible { get; set; }

    public bool ShouldSerializeName() => Name != null;
    public bool ShouldSerializeItems() => ItemIds.Count > 0;
    public bool ShouldSerializeEffect() => Effects.Count > 0;
    public bool ShouldSerializeJokerPossible() => JokerPossible;
    public bool ShouldSerializeZeroWeaponJokerPossible() => ZeroWeaponJokerPossible;
}