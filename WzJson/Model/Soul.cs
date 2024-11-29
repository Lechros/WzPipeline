using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Soul
{
    public required string Name { get; set; }
    
    public required string Skill { get; set; }
    
    public int ChargeFactor { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Magnificent { get; set; }

    public GearOption? Option { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public RandomOptions? Options { get; set; }

    public bool ShouldSerializeOption() => Option?.Count() > 0;

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class RandomOptions
    {
        public required GearOption AttackPower { get; set; }
        public required GearOption MagicPower { get; set; }
        public required GearOption AllStat { get; set; }
        public required GearOption MaxHp { get; set; }
        public required GearOption CriticalRate { get; set; }
        public required GearOption BossDamage { get; set; }
        public required GearOption IgnoreMonsterArmor { get; set; }
    }
}