using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Domains.Gear;

public class GearParser
{
    public IEnumerable<MalibGear> Parse(GearNode node, GearParseContext context)
    {
        if (node.Id is null)
        {
            yield break;
        }

        if (!context.GearStringData.TryGetValue(node.Id.Value.ToString(), out var strings) || strings.Name == null)
        {
            yield break;
        }

        if (node.IsCash)
        {
            yield break;
        }

        var type = GetGearType(node.Id.Value);

        var gear = new MalibGear
        {
            Id = node.Id.Value,
            Name = strings.Name,
            Desc = strings.Desc,
            Icon = node.Id.Value.ToString(),
            Type = type,
            Req = GetGearReq(node, type),
            Attributes = GetGearAttribute(node),
            BaseOption = GetBaseOption(node),
            ScrollUpgradeableCount = node.Properties.GetValueOrDefault(GearPropType.tuc),
            PotentialGrade =
                GetPotentialGradeFromFixedGrade(node.Properties.GetValueOrDefault(GearPropType.fixedGrade)),
            Potentials = GetGearPotentials(node.Options, context.ItemOptionData),
            ExceptionalUpgradeableCount = node.Properties.GetValueOrDefault(GearPropType.Etuc)
        };

        gear.Attributes.CanStarforce = (int)GetCanStarforce(gear, node);
        gear.Attributes.CanScroll = (int)GetCanScroll(gear, node);
        gear.Attributes.CanAddOption = (int)GetCanAddOption(gear, node);
        gear.Attributes.CanPotential = (int)GetCanPotential(gear, node);
        gear.Attributes.CanAdditionalPotential = (int)GetCanAdditionalPotential(gear, node);

        var fixedMaxStar = GetFixedMaxStar(gear.Id, context.AstraSubWeaponData);
        if (fixedMaxStar != null)
        {
            gear.Attributes.FixedMaxStar = fixedMaxStar;
            gear.Attributes.CanStarforce = (int)GearCapability.Can;
        }

        var fullJobs = GetFullJobs(gear.Id, context.AstraSubWeaponData);
        if (fullJobs != null)
        {
            if (gear.Req.Job.FullJobs != null && gear.Req.Job.FullJobs.Length > 0)
            {
                if (!gear.Req.Job.FullJobs.SequenceEqual(fullJobs))
                {
                    throw new InvalidDataException(
                        $"Gear {gear.Id} already has different fullJobs: {string.Join(",", gear.Req.Job.FullJobs)} != {string.Join(",", fullJobs)}");
                }
            }
            else
            {
                gear.Req.Job.FullJobs = fullJobs;
            }
        }

        var skills = GetSpecialWeaponSkills(gear);
        gear.Attributes.Skills.AddRange(skills.Select(skillId => context.SkillNameData[skillId.ToString()]));

        yield return gear;
    }

    private static GearType GetGearType(int code)
    {
        switch (code / 1000)
        {
            case 1098:
            case 1099:
            case 1212:
            case 1213:
            case 1214:
            case 1215:
            case 1216:
            case 1252:
            case 1253:
            case 1254:
            case 1259:
            case 1403:
            case 1404:
            case 1433:
            case 1712:
            case 1713:
            case 1714:
            case 1726:
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

    private static GearReq GetGearReq(GearNode node, GearType type)
    {
        return new GearReq
        {
            Level = node.Properties.GetValueOrDefault(GearPropType.reqLevel, 0),
            Job = GetGearReqJob(node),
            Gender = (int?)GetGearGender(node, type)
        };
    }

    private static GearReqJob GetGearReqJob(GearNode node)
    {
        var hasReqSpecJobs = false;
        var fullJobs = new List<int>();
        foreach (var reqSpecJob in node.ReqSpecJobs)
        {
            hasReqSpecJobs = true;
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
                default: throw new NotImplementedException($"Unknown reqSpecJob in {node.Id}: {reqSpecJob}");
            }
        }

        if (hasReqSpecJobs)
        {
            return new GearReqJob
            {
                Class = node.Properties.GetValueOrDefault(GearPropType.reqJob, 0),
                FullJobs = fullJobs.ToArray()
            };
        }
        else if (node.Properties.TryGetValue(GearPropType.reqSpecJob, out var value))
        {
            return new GearReqJob
            {
                Class = node.Properties.GetValueOrDefault(GearPropType.reqJob, 0),
                Jobs = [value]
            };
        }
        else
        {
            return new GearReqJob
            {
                Class = node.Properties.GetValueOrDefault(GearPropType.reqJob, 0),
            };
        }
    }

    private static GearGender? GetGearGender(GearNode node, GearType type)
    {
        switch (type)
        {
            case GearType.emblem:
            case GearType.powerSource:
            case GearType.jewel:
                return null;
        }

        var value = node.Id!.Value / 1000 % 10;
        return (GearGender)value switch
        {
            GearGender.Male => GearGender.Male,
            GearGender.Female => GearGender.Female,
            _ => null
        };
    }

    private static GearAttribute GetGearAttribute(GearNode node)
    {
        return new GearAttribute
        {
            Only = node.GetBooleanValue(GearPropType.only),
            Trade = (int)GetGearTrade(node),
            OnlyEquip = node.GetBooleanValue(GearPropType.onlyEquip),
            Share = (int)GetGearShare(node),
            Superior = node.GetBooleanValue(GearPropType.superiorEqp),
            AttackSpeed = node.Properties.GetValueOrDefault(GearPropType.attackSpeed),
            SpecialGrade = node.GetBooleanValue(GearPropType.specialGrade),
            Cuttable = node.Properties.GetValueOrDefault(GearPropType.tradeAvailable),
            CuttableCount = node.Properties.GetValueOrDefault(GearPropType.CuttableCount),
            TotalCuttableCount = node.Properties.GetValueOrDefault(GearPropType.CuttableCount),
            AccountShareTag = node.GetBooleanValue(GearPropType.accountShareTag),
            SetItemId = node.Properties.GetValueOrDefault(GearPropType.setItemID),
            Lucky = node.GetBooleanValue(GearPropType.jokerToSetItem),
            BossReward = node.GetBooleanValue(GearPropType.bossReward)
        };
    }

    private static GearTrade GetGearTrade(GearNode node)
    {
        if (node.GetBooleanValue(GearPropType.tradeBlock))
        {
            return GearTrade.TradeBlock;
        }

        if (node.GetBooleanValue(GearPropType.equipTradeBlock))
        {
            return GearTrade.EquipTradeBlock;
        }

        return 0;
    }

    private static GearShare GetGearShare(GearNode node)
    {
        if (node.GetBooleanValue(GearPropType.accountSharable))
        {
            return node.GetBooleanValue(GearPropType.sharableOnce)
                ? GearShare.AccountSharableOnce
                : GearShare.AccountSharable;
        }

        return 0;
    }

    private static GearOption GetBaseOption(GearNode node)
    {
        var baseOption = new GearOption();
        foreach (var (prop, value) in node.Properties)
        {
            baseOption.Add(prop, value);
        }

        return baseOption;
    }

    private static GearPotential[]? GetGearPotentials((int, int)[]? options, ItemOptionData itemOptionData)
    {
        if (options == null)
        {
            return null;
        }

        var potentials = new List<GearPotential>(options.Length);
        foreach (var (optionCode, level) in options)
        {
            var itemOption = itemOptionData[optionCode];
            var levelOption = itemOption.Level[level];
            var grade = levelOption.FixedGrade != null
                ? GetPotentialGradeFromFixedGrade(levelOption.FixedGrade.Value)
                : optionCode / 10000;
            var potential = new GearPotential
            {
                Id = optionCode,
                Grade = grade,
                Summary = levelOption.String,
                Option = levelOption.Option
            };
            potentials.Add(potential);
        }

        return potentials.ToArray();
    }

    private static int GetPotentialGradeFromFixedGrade(int fixedGrade)
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

    private static GearCapability GetCanStarforce(MalibGear gear, GearNode node)
    {
        if (gear.ScrollUpgradeableCount == 0)
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.onlyUpgrade))
        {
            return GearCapability.Cannot;
        }

        if (gear.Type.IsMechanicGear() || gear.Type.IsDragonGear())
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.exceptUpgrade))
        {
            return GearCapability.Fixed;
        }

        return GearCapability.Can;
    }

    private static GearCapability GetCanScroll(MalibGear gear, GearNode node)
    {
        if (gear.ScrollUpgradeableCount == 0)
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.exceptUpgrade))
        {
            return GearCapability.Fixed;
        }

        return GearCapability.Can;
    }

    private static GearCapability GetCanAddOption(MalibGear gear, GearNode node)
    {
        if (node.GetBooleanValue(GearPropType.exUpgradeBlock))
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.exUpgradeChangeBlock))
        {
            return GearCapability.Fixed;
        }

        if (node.GetBooleanValue(GearPropType.setExtraOption))
        {
            return GearCapability.Can;
        }

        return GearTypeSupportsAddOption(gear.Type) ? GearCapability.Can : GearCapability.Cannot;
    }

    private static bool GearTypeSupportsAddOption(GearType type)
    {
        if (type.IsWeapon())
        {
            return true;
        }

        if (type.IsArmor())
        {
            return !type.IsShield();
        }

        if (type.IsAccessory())
        {
            return type != GearType.ring && type != GearType.shoulder;
        }

        if (type == GearType.pocket)
        {
            return true;
        }

        return false;
    }

    private static GearCapability GetCanPotential(MalibGear gear, GearNode node)
    {
        if (node.GetBooleanValue(GearPropType.noPotential))
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.fixedPotential))
        {
            return GearCapability.Fixed;
        }

        if (gear.ScrollUpgradeableCount > 0 || node.GetBooleanValue(GearPropType.tucIgnoreForPotential))
        {
            return gear.Type.IsMechanicGear() || gear.Type.IsDragonGear()
                ? GearCapability.Cannot
                : GearCapability.Can;
        }

        return GearTypeSupportsPotential(gear.Type)
            ? GearCapability.Can
            : GearCapability.Cannot;
    }

    private static GearCapability GetCanAdditionalPotential(MalibGear gear, GearNode node)
    {
        if (node.GetBooleanValue(GearPropType.noPotential))
        {
            return GearCapability.Cannot;
        }

        if (node.GetBooleanValue(GearPropType.fixedPotential))
        {
            return GearCapability.Cannot;
        }

        if (gear.ScrollUpgradeableCount > 0 || node.GetBooleanValue(GearPropType.tucIgnoreForPotential))
        {
            return gear.Type.IsMechanicGear() || gear.Type.IsDragonGear()
                ? GearCapability.Cannot
                : GearCapability.Can;
        }

        return GearTypeSupportsPotential(gear.Type)
            ? GearCapability.Can
            : GearCapability.Cannot;
    }

    private static bool GearTypeSupportsPotential(GearType type)
    {
        if (type.IsSubWeapon())
        {
            return true;
        }

        switch (type)
        {
            case GearType.soulShield:
            case GearType.demonShield:
            case GearType.katara:
            case GearType.magicArrow:
            case GearType.card:
            case GearType.orb:
            case GearType.dragonEssence:
            case GearType.soulRing:
            case GearType.magnum:
            case GearType.emblem:
                return true;
            default:
                return false;
        }
    }

    private static int? GetFixedMaxStar(int gearId, AstraSubWeaponData astraSubWeaponData)
    {
        switch (astraSubWeaponData.GetAstraIndex(gearId))
        {
            case 0:
                return 15;
            case 1:
                return 20;
            case 2:
                return 30;
            default:
                return null;
        }
    }

    private static int[]? GetFullJobs(int gearId, AstraSubWeaponData astraSubWeaponData)
    {
        if (astraSubWeaponData.TryGetValue(gearId, out var entry))
        {
            // 썬콜, 불독, 비숍 순서가 되도록 보정
            if (entry.Jobs.SequenceEqual([212, 222, 232]))
            {
                return [222, 212, 232];
            }

            return entry.Jobs.ToArray();
        }

        return null;
    }

    private static int[] GetSpecialWeaponSkills(MalibGear gear)
    {
        if (gear.Type.IsWeapon() && gear.Attributes.SetItemId is >= 886 and <= 890)
        {
            return gear.Req.Level switch
            {
                // Genesis
                200 => [80002632, 80002633],
                // Destiny
                250 => [80003873, 80003874],
                _ => []
            };
        }

        return [];
    }
}

internal static class GearNodeExtensions
{
    public static bool GetBooleanValue(this GearNode node, GearPropType type)
    {
        return node.Properties.TryGetValue(type, out var value) && value != 0;
    }
}