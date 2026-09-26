using System.Reflection;
using System.Text;

namespace WzPipeline.Domains.Shared;

public class GearOption
{
    private static readonly PropertyInfo[] Properties;
    private static readonly Dictionary<string, PropertyInfo> PropertyMap;

    static GearOption()
    {
        Properties = typeof(GearOption).GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0)
            .ToArray();
        PropertyMap = Properties.ToDictionary(p => p.Name, p => p);
    }

    public double Str { get; set; }
    public double Dex { get; set; }
    public double Int { get; set; }
    public double Luk { get; set; }
    public double StrRate { get; set; }
    public double DexRate { get; set; }
    public double IntRate { get; set; }
    public double LukRate { get; set; }
    public double MaxHp { get; set; }
    public double MaxMp { get; set; }
    public double MaxHpRate { get; set; }
    public double MaxMpRate { get; set; }
    public double MaxDemonForce { get; set; }
    public double AttackPower { get; set; }
    public double MagicPower { get; set; }
    public double AttackPowerRate { get; set; }
    public double MagicPowerRate { get; set; }
    public double Armor { get; set; }
    public double ArmorRate { get; set; }
    public double Speed { get; set; }
    public double Jump { get; set; }
    public double BossDamage { get; set; }
    public double IgnoreMonsterArmor { get; set; }
    public double AllStat { get; set; }
    public double Damage { get; set; }
    public double ReqLevelDecrease { get; set; }
    public double CriticalRate { get; set; }
    public double CriticalDamage { get; set; }
    public double CooltimeReduce { get; set; }
    public double StrLv { get; set; }
    public double DexLv { get; set; }
    public double IntLv { get; set; }
    public double LukLv { get; set; }

    public double AllStatsSetter
    {
        set
        {
            Str = value;
            Dex = value;
            Int = value;
            Luk = value;
        }
    }

    public double AllStatRatesSetter
    {
        set
        {
            StrRate = value;
            DexRate = value;
            IntRate = value;
            LukRate = value;
        }
    }

    public double this[string optionName]
    {
        get
        {
            if (!PropertyMap.TryGetValue(optionName, out var property) || !property.CanRead)
                throw new ArgumentException("Invalid gear option name: " + optionName);
            return (double)property.GetValue(this)!;
        }
        set
        {
            if (!PropertyMap.TryGetValue(optionName, out var property) || !property.CanWrite)
                throw new ArgumentException("Invalid gear option name: " + optionName);
            property.SetValue(this, value);
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

    public bool Add(GearPropType prop, double value)
    {
        switch (prop)
        {
            case GearPropType.incSTR:
                Str += value;
                break;
            case GearPropType.incDEX:
                Dex += value;
                break;
            case GearPropType.incINT:
                Int += value;
                break;
            case GearPropType.incLUK:
                Luk += value;
                break;
            case GearPropType.incSTRr:
                StrRate += value;
                break;
            case GearPropType.incDEXr:
                DexRate += value;
                break;
            case GearPropType.incINTr:
                IntRate += value;
                break;
            case GearPropType.incLUKr:
                LukRate += value;
                break;
            case GearPropType.incMHP:
                MaxHp += value;
                break;
            case GearPropType.incMMP:
                MaxMp += value;
                break;
            case GearPropType.incMHPr:
                MaxHpRate += value;
                break;
            case GearPropType.incMMPr:
                MaxMpRate += value;
                break;
            case GearPropType.incPAD:
                AttackPower += value;
                break;
            case GearPropType.incMAD:
                MagicPower += value;
                break;
            case GearPropType.incPADr:
                AttackPowerRate += value;
                break;
            case GearPropType.incMADr:
                MagicPowerRate += value;
                break;
            case GearPropType.incPDD:
                Armor += value;
                break;
            case GearPropType.incPDDr:
                ArmorRate += value;
                break;
            case GearPropType.incSpeed:
                Speed += value;
                break;
            case GearPropType.incJump:
                Jump += value;
                break;
            case GearPropType.bdR:
            case GearPropType.incBDR:
                BossDamage += value;
                break;
            case GearPropType.imdR:
            case GearPropType.incIMDR:
            case GearPropType.ignoreTargetDEF:
                if (IgnoreMonsterArmor != 0 && value != 0)
                    throw new InvalidOperationException("Cannot add ignored monster armor");

                IgnoreMonsterArmor += value;
                break;
            case GearPropType.damR:
            case GearPropType.incDAMr:
                Damage += value;
                break;
            case GearPropType.reduceReq:
                ReqLevelDecrease += value;
                break;
            case GearPropType.incCr:
                CriticalRate += value;
                break;
            case GearPropType.incCDr:
            case GearPropType.criticaldamage:
            case GearPropType.incCriticaldamageF:
            case GearPropType.incCriticaldamage:
                CriticalDamage += value;
                break;
            case GearPropType.reduceCooltime:
                CooltimeReduce += value;
                break;
            case GearPropType.incSTRlv:
                StrLv += value;
                break;
            case GearPropType.incDEXlv:
                DexLv += value;
                break;
            case GearPropType.incINTlv:
                IntLv += value;
                break;
            case GearPropType.incLUKlv:
                LukLv += value;
                break;
            case GearPropType.incAllStat:
                Str += value;
                Dex += value;
                Int += value;
                Luk += value;
                break;
            default:
                return false;
        }

        return true;
    }

    public bool IsEmpty()
    {
        return Str == 0 && Dex == 0 && Int == 0 && Luk == 0 && StrRate == 0 && DexRate == 0 && IntRate == 0 &&
               LukRate == 0 && MaxHp == 0 && MaxMp == 0 && MaxHpRate == 0 && MaxMpRate == 0 && MaxDemonForce == 0 &&
               AttackPower == 0 && MagicPower == 0 && AttackPowerRate == 0 && MagicPowerRate == 0 && Armor == 0 &&
               ArmorRate == 0 && Speed == 0 && Jump == 0 && BossDamage == 0 && IgnoreMonsterArmor == 0 &&
               AllStat == 0 && Damage == 0 && ReqLevelDecrease == 0 && CriticalRate == 0 && CriticalDamage == 0 &&
               CooltimeReduce == 0 && StrLv == 0 && DexLv == 0 && IntLv == 0 && LukLv == 0;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("GearOption { ");
        var values = (from property in Properties
            where property.CanRead
            let value = (double)property.GetValue(this)!
            where value != 0
            select $"{property.Name}={value}").ToList();
        if (values.Count > 0)
        {
            sb.Append(' ');
            sb.Append(string.Join(", ", values));
            sb.Append(' ');
        }

        sb.Append('}');
        return sb.ToString();
    }
}