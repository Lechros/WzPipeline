using System.Reflection;

namespace WzJson.Domains.Gear.Models;

public class GearOption
{
    public int Str { get; set; }
    public int Dex { get; set; }
    public int Int { get; set; }
    public int Luk { get; set; }
    public int StrRate { get; set; }
    public int DexRate { get; set; }
    public int IntRate { get; set; }
    public int LukRate { get; set; }
    public int MaxHp { get; set; }
    public int MaxMp { get; set; }
    public int MaxHpRate { get; set; }
    public int MaxMpRate { get; set; }
    public int MaxDemonForce { get; set; }
    public int AttackPower { get; set; }
    public int MagicPower { get; set; }
    public int AttackPowerRate { get; set; }
    public int MagicPowerRate { get; set; }
    public int Armor { get; set; }
    public int ArmorRate { get; set; }
    public int Speed { get; set; }
    public int Jump { get; set; }
    public int BossDamage { get; set; }
    public int IgnoreMonsterArmor { get; set; }
    public int AllStat { get; set; }
    public int Damage { get; set; }
    public int ReqLevelDecrease { get; set; }
    public int CriticalRate { get; set; }
    public int CriticalDamage { get; set; }
    public int CooltimeReduce { get; set; }
    public int StrLv { get; set; }
    public int DexLv { get; set; }
    public int IntLv { get; set; }
    public int LukLv { get; set; }

    public int AllStatsSetter
    {
        set
        {
            Str = value;
            Dex = value;
            Int = value;
            Luk = value;
        }
    }

    public int AllStatRatesSetter
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

    public bool Add(GearPropType prop, int value)
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
                {
                    throw new InvalidOperationException("Cannot add ignored monster armor");
                }

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

    private static readonly Dictionary<string, PropertyInfo> Properties;

    static GearOption()
    {
        Properties = typeof(GearOption).GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p);
    }
}