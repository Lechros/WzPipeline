using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WzJson.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class GearOption
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Str { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Dex { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Int { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Luk { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int StrRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int DexRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int IntRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int LukRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxHp { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxMp { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxHpRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxMpRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int AttackPower { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MagicPower { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int AttackPowerRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MagicPowerRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Armor { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ArmorRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Speed { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Jump { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int BossDamage { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int IgnoreMonsterArmor { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int AllStat { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Damage { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ReqLevelDecrease { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int CriticalRate { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int CriticalDamage { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int CooltimeReduce { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int StrLv { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int DexLv { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int IntLv { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int LukLv { get; set; }

    [JsonIgnore]
    public int _AllStatsSetter
    {
        set
        {
            Str = value;
            Dex = value;
            Int = value;
            Luk = value;
        }
    }

    [JsonIgnore]
    public int _AllStatRatesSetter
    {
        set
        {
            StrRate = value;
            DexRate = value;
            IntRate = value;
            LukRate = value;
        }
    }

    public void Add(GearOption option)
    {
        Str += option.Str;
        Dex += option.Dex;
        Int += option.Int;
        Luk += option.Luk;
        StrRate += option.StrRate;
        DexRate += option.DexRate;
        IntRate += option.IntRate;
        LukRate += option.LukRate;
        MaxHp += option.MaxHp;
        MaxMp += option.MaxMp;
        MaxHpRate += option.MaxHpRate;
        MaxMpRate += option.MaxMpRate;
        AttackPower += option.AttackPower;
        MagicPower += option.MagicPower;
        AttackPowerRate += option.AttackPowerRate;
        MagicPowerRate += option.MagicPowerRate;
        Armor += option.Armor;
        ArmorRate += option.ArmorRate;
        Speed += option.Speed;
        Jump += option.Jump;
        BossDamage += option.BossDamage;
        IgnoreMonsterArmor += option.IgnoreMonsterArmor;
        AllStat += option.AllStat;
        Damage += option.Damage;
        ReqLevelDecrease += option.ReqLevelDecrease;
        CriticalRate += option.CriticalRate;
        CriticalDamage += option.CriticalDamage;
        CooltimeReduce += option.CooltimeReduce;
        StrLv += option.StrLv;
        DexLv += option.DexLv;
        IntLv += option.IntLv;
        LukLv += option.LukLv;
    }

    public int this[string optionName]
    {
        get
        {
            if (!Properties.TryGetValue(optionName, out var property) || !property.CanRead)
                throw new ArgumentException("Invalid gear option name: " + optionName);
            return (int)property.GetValue(this)!;
        }
        set
        {
            if (!Properties.TryGetValue(optionName, out var property) || !property.CanWrite)
                throw new ArgumentException("Invalid gear option name: " + optionName);
            property.SetValue(this, value);
        }
    }

    private static readonly IReadOnlyDictionary<string, PropertyInfo> Properties;

    static GearOption()
    {
        Properties = typeof(GearOption).GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p);
    }
}