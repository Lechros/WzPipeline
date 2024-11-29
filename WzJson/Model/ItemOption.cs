using Newtonsoft.Json;

namespace WzJson.Model;

public class ItemOption
{
    [JsonProperty(Order = 1, PropertyName = "optionType")]
    public int OptionType { get; set; }

    [JsonProperty(Order = 2, PropertyName = "reqLevel")]
    public int ReqLevel { get; set; }

    [JsonProperty(Order = 3, PropertyName = "string")]
    public string? String { get; set; }

    [JsonProperty(Order = 4, PropertyName = "level")]
    public SortedDictionary<int, Dictionary<string, object>> Level { get; set; } = new();

    public bool ShouldSerializeOptionType() => OptionType > 0;
    public bool ShouldSerializeReqLevel() => ReqLevel > 0;
    public bool ShouldSerializeString() => String != null;
    public bool ShouldSerializeLevel() => Level.Count > 0;
}