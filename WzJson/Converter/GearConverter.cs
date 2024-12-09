using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public class GearConverter(
    string dataLabel,
    string dataPath,
    GlobalStringData globalStringData,
    JsonData<ItemOption> itemOptionData,
    GlobalFindNodeFunction findNode)
    : AbstractNodeConverter<Gear>
{
    public override IKeyValueData NewData() => new JsonData<Gear>(dataLabel, dataPath);

    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override Gear? ConvertNode(Wz_Node node, string key)
    {
        globalStringData.Eqp.TryGetValue(key, out var gearString);
        if (gearString?.Name == null) return null;
        var infoNode = node.FindNodeByPath("info");
        if (infoNode == null) return null;
        var cashNode = infoNode.FindNodeByPath("cash");
        if (cashNode != null && cashNode.GetValue<int>() != 0) return null;

        var gearId = int.Parse(key);
        var gear = new Gear
        {
            Meta = { Id = gearId },
            Name = gearString.Name,
            Desc = gearString.Desc,
            Type = GetGearType(gearId)
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
                case "addition":
                    break;
                case "option":
                    gear.Potentials = ConvertToGearPotentials(propNode);
                    break;
                default:
                    HandlePropNodeDefault(gear, propNode);
                    break;
            }
        }

        if (gear.Attributes._SharableOnce && gear.Attributes.Share == GearAttribute.GearShare.AccountSharable)
            gear.Attributes.Share = GearAttribute.GearShare.AccountSharableOnce;

        if (gear.Potentials != null && gear.Potentials.Any(p => p != null) &&
            gear.PotentialGrade == PotentialGrade.Normal)
            gear.PotentialGrade = PotentialGrade.Rare;

        gear.MaxStar = GetGearMaxStar(gear);

        return gear;
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
            case 1404:
                return GearType.chakram;
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

    private static readonly int[][] MaxStarData =
    [
        [0, 5, 3],
        [95, 8, 5],
        [110, 10, 8],
        [120, 15, 10],
        [130, 20, 12],
        [140, 25, 15]
    ];

    private int GetGearMaxStar(Gear gear)
    {
        if (gear.ScrollUpgradeableCount <= 0) return 0;
        if (gear.Attributes._OnlyUpgrade) return 0;
        if (gear.Type.IsMechanicGear() || gear.Type.IsDragonGear()) return 0;

        int[]? data = null;
        foreach (var item in MaxStarData)
        {
            if (gear.Req.Level >= item[0])
                data = item;
            else
                break;
        }

        if (data == null) return 0;
        return gear.Attributes.Superior ? data[2] : data[1];
    }

    private GearPotential?[] ConvertToGearPotentials(Wz_Node propNode)
    {
        GearPotential?[] potentials = [null, null, null];
        for (var i = 0; i < propNode.Nodes.Count; i++)
        {
            var optionNode = propNode.Nodes[i];
            var optionCode = optionNode.Nodes["option"].GetValue<string>();
            var level = optionNode.Nodes["level"].GetValue<int>();
            var itemOption = itemOptionData[optionCode];
            potentials[i] = new GearPotential
            {
                Title = itemOption.Level[level].String,
                Option = itemOption.Level[level].Option
            };
        }

        return potentials;
    }

    private void HandlePropNodeDefault(Gear gear, Wz_Node propNode)
    {
        if (int.TryParse(propNode.Text, out _)) return;
        if (!Enum.TryParse(propNode.Text, out GearPropType propType)) return;

        if (propType == GearPropType.onlyUpgrade)
        {
            gear.Attributes._OnlyUpgrade = true;
            return;
        }

        var value = propNode.GetValue<int>();
        if (value == 0) return;

        var optionName = propType.GetGearOptionName();
        if (optionName != null)
        {
            gear.BaseOption[optionName] = propNode.GetValue<int>();
            return;
        }

        switch (propType)
        {
            // Req
            case GearPropType.reqLevel: gear.Req.Level = value; break;
            case GearPropType.reqSTR: gear.Req.Str = value; break;
            case GearPropType.reqLUK: gear.Req.Luk = value; break;
            case GearPropType.reqDEX: gear.Req.Dex = value; break;
            case GearPropType.reqINT: gear.Req.Int = value; break;
            case GearPropType.reqJob: gear.Req.Job = value; break;
            case GearPropType.reqSpecJob: gear.Req.Class = value; break;

            // Equip, trade
            case GearPropType.only:
                gear.Attributes.Only = true;
                break;
            case GearPropType.tradeBlock:
                gear.Attributes.Trade = GearAttribute.GearTrade.TradeBlock;
                break;
            case GearPropType.equipTradeBlock:
                if (gear.Attributes.Trade != GearAttribute.GearTrade.TradeBlock)
                    gear.Attributes.Trade = GearAttribute.GearTrade.EquipTradeBlock;
                break;
            case GearPropType.onlyEquip:
                gear.Attributes.OnlyEquip = true;
                break;
            case GearPropType.sharableOnce:
                gear.Attributes._SharableOnce = true;
                break;
            case GearPropType.accountSharable:
                gear.Attributes.Share = GearAttribute.GearShare.AccountSharable;
                break;
            case GearPropType.tradeAvailable:
                gear.Attributes.Cuttable = (GearAttribute.GearCuttable)value;
                break;
            case GearPropType.CuttableCount:
                gear.Attributes.CuttableCount = value;
                break;
            case GearPropType.accountShareTag:
                gear.Attributes.AccountShareTag = true;
                break;

            // Etc
            case GearPropType.jokerToSetItem:
                gear.Attributes.Lucky = true;
                break;
            case GearPropType.bossReward:
                gear.Attributes.BossReward = true;
                break;

            // AddOption
            case GearPropType.exUpgradeBlock:
                gear.Attributes.CanAddOption = GearAttribute.AddOptionCan.Cannot;
                break;
            case GearPropType.exUpgradeChangeBlock:
                gear.Attributes.CanAddOption = GearAttribute.AddOptionCan.Cannot;
                break;

            // Upgrade
            case GearPropType.blockGoldHammer:
                gear.Attributes.BlockGoldenHammer = true;
                break;
            case GearPropType.superiorEqp:
                gear.Attributes.Superior = true;
                break;
            case GearPropType.exceptUpgrade:
                gear.Attributes.CannotUpgrade = true;
                break;
            case GearPropType.tuc:
                gear.ScrollUpgradeableCount = value;
                break;

            // Starforce
            case GearPropType.incCHUC:
                gear.Star = value;
                break;

            // Potential
            case GearPropType.specialGrade:
                gear.Attributes.SpecialGrade = true;
                break;
            case GearPropType.noPotential:
                gear.Attributes.CanPotential = GearAttribute.PotentialCan.Cannot;
                break;
            case GearPropType.fixedPotential:
                gear.Attributes.CanPotential = GearAttribute.PotentialCan.Fixed;
                gear.Attributes.CanAdditionalPotential = GearAttribute.PotentialCan.Cannot;
                break;
            case GearPropType.tucIgnoreForPotential:
                if (gear.Attributes.CanPotential == GearAttribute.PotentialCan.None)
                    gear.Attributes.CanPotential = GearAttribute.PotentialCan.Can;
                break;
            case GearPropType.fixedGrade:
                gear.PotentialGrade = value switch
                {
                    2 => PotentialGrade.Rare,
                    3 => PotentialGrade.Epic,
                    5 => PotentialGrade.Unique,
                    7 => PotentialGrade.Legendary,
                    _ => (PotentialGrade)(value - 1)
                };
                break;

            // Attributes: incline
            case GearPropType.charismaEXP: gear.Attributes.Incline.Charisma = value; break;
            case GearPropType.senseEXP: gear.Attributes.Incline.Sense = value; break;
            case GearPropType.insightEXP: gear.Attributes.Incline.Insight = value; break;
            case GearPropType.willEXP: gear.Attributes.Incline.Will = value; break;
            case GearPropType.craftEXP: gear.Attributes.Incline.Craft = value; break;
            case GearPropType.charmEXP: gear.Attributes.Incline.Charm = value; break;

            // Exceptional
            case GearPropType.Etuc:
                gear.ExceptionalUpgradeableCount = value;
                break;
        }
    }
}