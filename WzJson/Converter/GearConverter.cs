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

        return gear;
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