using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;

namespace WzPipeline.Domains.Gear.Processors;

public class RawGearToGearProcessor : AbstractProcessor<RawGear, MalibGear>
{
    public override IEnumerable<MalibGear> Process(IEnumerable<RawGear> models)
    {
        foreach (var raw in models)
        {
            yield return Process(raw);
        }
    }

    private MalibGear Process(RawGear raw)
    {
        return new MalibGear
        {
            Meta = new GearMeta(raw.Id),
            Name = raw.Name,
            Desc = raw.Desc,
            Type = GetGearType(raw.Id),
            Req = GetGearReq(raw),
            Attributes = new GearAttribute(),
            RawAttributes = GetRawAttributes(raw),
            BaseOption = GetBaseOption(raw),
            ScrollUpgradeableCount = raw.Props.GetValueOrDefault(GearPropType.tuc, 0),
            PotentialGrade = GetPotentialGradeFromFixedGrade(raw),
            Potentials = raw.Potentials,
            ExceptionalUpgradeableCount = raw.Props.GetValueOrDefault(GearPropType.Etuc, 0),
        };
    }

    private GearType GetGearType(int code)
    {
        switch (code / 1000)
        {
            case 1098:
            case 1099:
            case 1212:
            case 1213:
            case 1214:
            case 1215:
            case 1252:
            case 1253:
            case 1259:
            case 1403:
            case 1404:
            case 1712:
            case 1713:
            case 1714:
                return (GearType)(code / 1000);
        }

        if (code / 10000 == 135)
        {
            switch (code / 100)
            {
                case 13522:
                case 13528:
                case 13529:
                case 13540:
                    return (GearType)(code / 10);

                default:
                    return (GearType)(code / 100 * 10);
            }
        }

        if (code / 10000 == 119)
        {
            switch (code / 100)
            {
                case 11902:
                    return (GearType)(code / 10);
            }
        }

        // MSN support
        if (code / 10000 == 179)
        {
            switch (code / 1000)
            {
                case 1790:
                case 1791:
                case 1792:
                case 1793:
                    return (GearType)(code / 1000);
                default:
                    return (GearType)(code / 100 * 10);
            }
        }

        return (GearType)(code / 10000);
    }

    private GearReq GetGearReq(RawGear raw)
    {
        return new GearReq
        {
            Level = raw.Props.GetValueOrDefault(GearPropType.reqLevel, 0),
            Job = raw.Props.GetValueOrDefault(GearPropType.reqJob, 0),
            Class = raw.Props.GetValueOrDefault(GearPropType.reqSpecJob, 0),
        };
    }

    private Dictionary<GearPropType, int> GetRawAttributes(RawGear raw)
    {
        return raw.Props
            .Where(kv => AttributeTypes.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    private GearOption GetBaseOption(RawGear raw)
    {
        var baseOption = new GearOption();
        foreach (var (prop, value) in raw.Props)
        {
            baseOption.Add(prop, value);
        }

        return baseOption;
    }

    private int GetPotentialGradeFromFixedGrade(RawGear raw)
    {
        if (raw.Props.TryGetValue(GearPropType.fixedGrade, out var fixedGrade))
        {
            return fixedGrade switch
            {
                2 => 1,
                3 => 2,
                5 => 3,
                7 => 4,
                _ => fixedGrade - 1
            };
        }

        return 0;
    }

    private static readonly HashSet<GearPropType> AttributeTypes =
    [
        GearPropType.attackSpeed,
        GearPropType.setItemID,
        GearPropType.only,
        GearPropType.tradeBlock,
        GearPropType.accountSharable,
        GearPropType.onlyEquip,
        GearPropType.tradeAvailable,
        GearPropType.equipTradeBlock,
        GearPropType.sharableOnce,
        GearPropType.notExtend,
        GearPropType.accountShareTag,
        GearPropType.noPotential,
        GearPropType.fixedPotential,
        GearPropType.specialGrade,
        GearPropType.superiorEqp,
        GearPropType.jokerToSetItem,
        GearPropType.blockGoldHammer,
        GearPropType.exceptUpgrade,
        GearPropType.onlyUpgrade,
        GearPropType.noLookChange,
        GearPropType.tucIgnoreForPotential,
        GearPropType.CuttableCount,
        GearPropType.exUpgradeBlock,
        GearPropType.exUpgradeChangeBlock,
        GearPropType.bossReward
    ];
}