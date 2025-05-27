using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Gear
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Icon { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Desc { get; set; }

    public GearPotential[]? Potentials { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonExtensionData]
    public Dictionary<string, JToken> Props { get; set; } = new();
    
    public string[]? Skills { get; set; }

    public bool ShouldSerializePotentials() => Potentials != null;
    
    public bool ShouldSerializeSkills() => Skills != null;
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearPotential
{
    public required int Id { get; set; }
    
    public required int Grade { get; set; }
    
    public required string Summary { get; set; }

    public GearOption Option { get; set; } = new();
}