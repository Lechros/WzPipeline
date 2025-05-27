using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.DataProvider;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public class GearConverter(
    GlobalStringDataProvider globalStringDataProvider,
    ItemOptionDataProvider itemOptionDataProvider,
    GlobalFindNodeFunction findNode)
    : AbstractNodeConverter<Gear>
{
    private static readonly HashSet<string> Ignore =
    [
        "islot",
        "vslot",
        "price",
        "slotMax",
        "notSale",
        "noDrop",
        "undecomposable",
        "unsyntesizable",
        "exItem",
        "exGrade",
        "epic",
        "expireOnLogout",
        "expireOnNonPremiumLogin",
        "variableStat",
        "exceptToadsHammer",
        "exceptTransmission",
        "cubeExBaseOptionLevel",
        "gatherTool",
        "invisibleFace",
        "collabo",
        "equipDrop",
        "specialID",
        "tutorial",

        "icnSTR",
        "tradBlock",
        "bonusExp",
        "MaxHP",
        "speed",
        "incHP",
        "addtion",
        "additon",
        "noExtend",
        "hitDamRatePlus",
        "hitDamRatePlus2",

        "sfx",
        "walk",
        "stand",
        "attack",
        "afterImage",

        "info",
        "recovery",
        "jewelCraft",
        "scope",
        "replace",
        "reissueBan",
        "StarPlanet",
        "dayOfWeekItemStat",
        "randVariation",
        "icon2",
        "icon3",
        "icon4",
        "icon5",
        "icon6",
        "icon7",
        "iconRaw2",
        "iconRaw3",
        "iconRaw4",
        "iconRaw5",
        "iconRaw6",
        "iconRaw7",
        "iconD",
        "iconRawD",
        "toolTipPreview",
        "dialogPreview",
        "androidAni",
        "removeEar",
        "quest",
        "noPotentialFieldtype",
        "lookChangeType",
        "sample",
        "linkedPairItem",
        "chatBalloon",
        "effect",
        "reqSpecJobs",
        "pmdR",
        "fs",
        "head",
        "scanTradeBlock",
        "kaiserOffsetY",
        "incAttackCount",
        "kaiserOffsetX",
    ];

    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override Gear? Convert(Wz_Node node, string key)
    {
        globalStringDataProvider.Data.Eqp.TryGetValue(key, out var gearString);
        if (gearString?.Name == null) return null;
        var infoNode = node.FindNodeByPath("info");
        if (infoNode == null) return null;
        var cashNode = infoNode.FindNodeByPath("cash");
        if (cashNode != null && cashNode.GetValue<int>() != 0) return null;

        var gearId = int.Parse(key);
        var gear = new Gear
        {
            Id = gearId,
            Name = gearString.Name,
            Desc = gearString.Desc,
        };
        foreach (var propNode in infoNode.Nodes)
        {
            switch (propNode.Text)
            {
                case "icon":
                    var resolvedIconNode = WzUtility.ResolveLinkedNode(propNode, findNode);
                    var linkedGearNode = resolvedIconNode.ParentNode.ParentNode;
                    gear.Icon = WzUtility.GetNodeCode(linkedGearNode);
                    break;
                case "iconRaw":
                    break;
                case "addition":
                    break;
                case "option":
                    gear.Potentials = ConvertToGearPotentials(propNode);
                    break;
                case "onlyUpgrade":
                    gear.Props[GearPropType.onlyUpgrade.ToString()] = 1;
                    break;
                default:
                    HandlePropNodeDefault(gear, propNode);
                    break;
            }
        }

        var skillIds = new List<int>();
        if (IsDestinyWeapon(gear))
        {
            skillIds.Add(80003873);
            skillIds.Add(80003874);
        }
        else if (IsGenesisWeapon(gear))
        {
            skillIds.Add(80002632);
            skillIds.Add(80002633);
        }

        if (skillIds.Count > 0)
        {
            gear.Skills = skillIds.Select(skillId => globalStringDataProvider.Data.Skill[skillId.ToString()].Name!)
                .ToArray();
        }

        return gear;
    }

    private bool IsGenesisWeapon(Gear gear)
    {
        var type = GetGearType(gear.Id);
        if (!type.IsWeapon()) return false;
        if (!gear.Props.TryGetValue(nameof(GearPropType.setItemID), out var setItemIdToken)) return false;
        var setItemId = setItemIdToken.ToObject<int>();
        if (886 > setItemId || setItemId > 890) return false;
        return true;
    }

    private bool IsDestinyWeapon(Gear gear)
    {
        var type = GetGearType(gear.Id);
        if (!type.IsWeapon()) return false;
        if (!gear.Props.TryGetValue(nameof(GearPropType.setItemID), out var setItemIdToken)) return false;
        var setItemId = setItemIdToken.ToObject<int>();
        if (886 > setItemId || setItemId > 890) return false;
        if (!gear.Props.TryGetValue(nameof(GearPropType.reqLevel), out var reqLevelToken)) return false;
        var reqLevel = reqLevelToken.ToObject<int>();
        return reqLevel == 250;
    }

    private GearType GetGearType(int code)
    {
        switch (code / 1000)
        {
            case 1098:
                return GearType.soulShield;
            case 1099:
                return GearType.demonShield;
            case 1212:
                return GearType.shiningRod;
            case 1213:
                return GearType.tuner;
            case 1214:
                return GearType.breathShooter;
            case 1252:
            case 1259:
                return (GearType)(code / 1000);
            case 1403:
                return GearType.boxingCannon;
            case 1404:
                return GearType.chakram;
            case 1712:
                return GearType.arcaneSymbol;
            case 1713:
                return GearType.authenticSymbol;
            case 1714:
                return GearType.grandAuthenticSymbol;
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

        return (GearType)(code / 10000);
    }

    private GearPotential[] ConvertToGearPotentials(Wz_Node propNode)
    {
        var potentials = new List<GearPotential>(3);
        foreach (var optionNode in propNode.Nodes)
        {
            var optionCode = optionNode.Nodes["option"].GetValue<int>();
            var level = optionNode.Nodes["level"].GetValue<int>();
            var levelInfo = itemOptionDataProvider.Data.GetItemOptionLevelInfo(optionCode, level);
            potentials.Add(new GearPotential
            {
                Id = optionCode,
                Grade = optionCode / 10000,
                Summary = levelInfo.String,
                Option = levelInfo.Option
            });
        }

        return potentials.ToArray();
    }

    private void HandlePropNodeDefault(Gear gear, Wz_Node propNode)
    {
        if (int.TryParse(propNode.Text, out _)) return;
        if (Ignore.Contains(propNode.Text)) return;
        if (!Enum.TryParse(propNode.Text, out GearPropType propType))
        {
            throw new ArgumentException($"Unknown GearPropType in {gear.Id}: {propNode.Text}");
        }

        var value = propNode.GetValue<int>();
        if (value != 0)
        {
            gear.Props[propType.ToString()] = value;
        }
    }
}