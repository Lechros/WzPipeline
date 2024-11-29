using Newtonsoft.Json;

namespace WzJson.Model;

public class Soul
{
    [JsonProperty(Order = 1, PropertyName = "name")]
    public required string Name { get; set; }

    [JsonProperty(Order = 2, PropertyName = "skill")]
    public required string Skill { get; set; }

    [JsonProperty(Order = 3, PropertyName = "multiplier")]
    public int ChargeFactor { get; set; }

    [JsonProperty(Order = 4, PropertyName = "magnificent")]
    public bool Magnificent { get; set; }

    [JsonProperty(Order = 5, PropertyName = "option")]
    public GearOption? Option { get; set; }

    [JsonProperty(Order = 6, PropertyName = "options")]
    public RandomOptions? Options { get; set; }

    public bool ShouldSerializeMagnificent() => Magnificent;
    public bool ShouldSerializeOption() => Option?.Count() > 0;
    public bool ShouldSerializeOptions() => Options != null;

    public class RandomOptions
    {
        [JsonProperty(Order = 1, PropertyName = "attackPower")]
        public required GearOption AttackPower { get; set; }

        [JsonProperty(Order = 2, PropertyName = "magicPower")]
        public required GearOption MagicPower { get; set; }

        [JsonProperty(Order = 3, PropertyName = "allStat")]
        public required GearOption AllStat { get; set; }

        [JsonProperty(Order = 4, PropertyName = "maxHp")]
        public required GearOption MaxHp { get; set; }

        [JsonProperty(Order = 5, PropertyName = "criticalRate")]
        public required GearOption CriticalRate { get; set; }

        [JsonProperty(Order = 6, PropertyName = "bossDamage")]
        public required GearOption BossDamage { get; set; }

        [JsonProperty(Order = 7, PropertyName = "ignoreMonsterArmor")]
        public required GearOption IgnoreMonsterArmor { get; set; }
    }
}