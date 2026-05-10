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
            Id = raw.Id,
            Name = raw.Name,
            Desc = raw.Desc,
            Icon = raw.Id.ToString(),
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

        // 아스트라 보조무기
        if (code / 10000 == 172)
        {
            var index = (code % 10000) / 100;
            var typeList = new[]
            {
                GearType.medallion, GearType.rosary, GearType.ironChain, // 0 1 2
                GearType.magicBook1, GearType.magicBook2, GearType.magicBook3, // 3 4 5
                GearType.arrowFletching, GearType.bowThimble, GearType.relic, // 6 7 8
                GearType.charm, GearType.daggerScabbard, // 9 10
                GearType.wristBand, GearType.farSight, GearType.powderKeg, // 11 12 13
                GearType.jewel, GearType.jewel, GearType.jewel, GearType.jewel, GearType.jewel,
                GearType.jewel, // 14 15 16 17 18 19
                GearType.mass, GearType.magicArrow, GearType.card, GearType.orb, GearType.foxMarble,
                GearType.document, // 20 21 22 23 24 25
                GearType.demonShield, GearType.magicMarble, GearType.arrowhead, GearType.magnum, GearType.controller,
                GearType.charge, GearType.demonShield, // 26 27 28 29 30 31 32
                GearType.dragonEssence, GearType.weaponBelt, GearType.transmitter, GearType.soulRing, // 33 34 35 36
                GearType.hourGlass, GearType.chessPiece, // 37 38
                GearType.bracelet, GearType.magicWing, GearType.hexSeeker, GearType.pathOfAbyss, // 39 40 41 42
                GearType.sacredJewel, GearType.ornament, GearType.fanTassel, // 43 44 45
            };
            return typeList[index];
        }

        return (GearType)(code / 10000);
    }

    private GearReq GetGearReq(RawGear raw)
    {
        return new GearReq
        {
            Level = raw.Props.GetValueOrDefault(GearPropType.reqLevel, 0),
            Job = GetGearReqJob(raw)
        };
    }

    private GearReqJob GetGearReqJob(RawGear raw)
    {
        if (raw.ReqSpecJobs != null && raw.ReqSpecJobs.Length > 0)
        {
            var fullJobs = new List<int>();
            foreach (var reqSpecJob in raw.ReqSpecJobs)
            {
                switch (reqSpecJob)
                {
                    case 1: break;
                    case 2: break;
                    case 4: break;
                    case 11: fullJobs.Add(1112); break;
                    case 12: fullJobs.Add(1212); break;
                    case 22: fullJobs.Add(2217); break;
                    case 32: fullJobs.Add(3212); break;
                    case 36: break; // 제논 컨트롤러
                    default: throw new NotImplementedException($"Unknown reqSpecJob in {raw.Id}: {reqSpecJob}");
                }
            }

            return new GearReqJob
            {
                Class = raw.Props.GetValueOrDefault(GearPropType.reqJob, 0),
                FullJobs = fullJobs.ToArray()
            };
        }
        else if (raw.Props.TryGetValue(GearPropType.reqSpecJob, out var value))
        {
            return new GearReqJob
            {
                Class = raw.Props.GetValueOrDefault(GearPropType.reqJob, 0),
                Jobs = [value]
            };
        }
        else
        {
            return new GearReqJob
            {
                Class = raw.Props.GetValueOrDefault(GearPropType.reqJob, 0),
            };
        }
    }

    private Dictionary<GearPropType, int> GetRawAttributes(RawGear raw)
    {
        return raw.Props
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
}