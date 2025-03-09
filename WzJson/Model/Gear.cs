using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WzJson.Domain;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Gear
{
    public GearMetadata Meta { get; set; } = new();

    public required string Name { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Icon { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Desc { get; set; }

    public GearType Type { get; set; }

    public GearReq Req { get; set; } = new();

    public GearAttribute Attributes { get; set; } = new();

    public GearOption BaseOption { get; set; } = new();

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ScrollUpgradeableCount { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxStar { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Star { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public PotentialGrade PotentialGrade { get; set; }

    public GearPotential?[]? Potentials { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ExceptionalUpgradeableCount { get; set; }

    public bool ShouldSerializePotentials() => Potentials != null && Potentials.Any(p => p != null);
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearMetadata
{
    public int Id { get; set; }
    public int Version { get; set; } = 1;
    public object[] Add { get; set; } = [];
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearAttribute
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Only { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public GearTrade Trade { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool OnlyEquip { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public GearShare Share { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool BlockGoldenHammer { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Superior { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int AttackSpeed { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool CannotUpgrade { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public AddOptionCan CanAddOption { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public PotentialCan CanPotential { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public PotentialCan CanAdditionalPotential { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool SpecialGrade { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ReqLevelIncrease { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public GearCuttable Cuttable { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int CuttableCount { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool AccountShareTag { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Lucky { get; set; }

    public GearIncline Incline { get; set; } = new();

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool BossReward { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int GrowthExp { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int GrowthLevel { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? DateExpire { get; set; }

    [JsonIgnore] public bool _OnlyUpgrade { get; set; }
    [JsonIgnore] public bool _SharableOnce { get; set; }

    public bool ShouldSerializeIncline() => !Incline.IsEmpty();

    public enum GearTrade
    {
        Tradeable = 0,
        TradeBlock = 1,
        EquipTradeBlock = 2
    }

    public enum GearShare
    {
        None = 0,
        AccountSharable = 1,
        AccountSharableOnce = 2
    }

    public enum AddOptionCan
    {
        None = 0,
        Can = 1,
        Cannot = 2,
        Fixed = 3,
    }

    public enum PotentialCan
    {
        None = 0,
        Can = 1,
        Cannot = 2,
        Fixed = 3
    }

    public enum GearCuttable
    {
        None = 0,
        Silver = 1,
        Platinum = 2
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class GearIncline
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Charisma { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Insight { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Will { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Craft { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Sense { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Charm { get; set; }

        public bool IsEmpty()
        {
            return Charisma == 0 && Insight == 0 && Will == 0 && Craft == 0 && Sense == 0 && Charm == 0;
        }
    }
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearReq
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Level { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Str { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Luk { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Dex { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Int { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Job { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Class { get; set; }
}

public enum PotentialGrade
{
    Normal = 0,
    Rare = 1,
    Epic = 2,
    Unique = 3,
    Legendary = 4
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearPotential
{
    public required string Title { get; set; }

    public GearOption Option { get; set; } = new();
}