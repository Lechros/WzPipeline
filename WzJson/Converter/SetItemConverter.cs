using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.DataProvider;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public class SetItemConverter(ItemOptionDataProvider itemOptionDataProvider) : AbstractNodeConverter<SetItem>
{
    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override SetItem? Convert(Wz_Node node, string _)
    {
        var setItem = new SetItem();
        foreach (var subNode in node.Nodes)
        {
            switch (subNode.Text)
            {
                case "setItemName":
                    setItem.Name = subNode.GetValue<string>();
                    break;
                case "ItemID":
                    foreach (var itemNode in subNode.Nodes)
                    {
                        // ItemID: { *1: id*, 2: id, ... }
                        if (itemNode.Nodes.Count == 0)
                        {
                            setItem.ItemIds.Add(itemNode.GetValue<int>());
                        }
                        // ItemID: { *1: { 1: id, 2: id, typeName: ... }*, 2: ..., ... }
                        else
                        {
                            foreach (var itemPartNode in itemNode.Nodes)
                            {
                                switch (itemPartNode.Text)
                                {
                                    case "representName":
                                    case "typeName":
                                    case "byGender":
                                        break;
                                    default:
                                        if (int.TryParse(itemPartNode.Text, out var nodeIndex) && nodeIndex > 0)
                                            setItem.ItemIds.Add(itemPartNode.GetValue<int>());
                                        break;
                                }
                            }
                        }
                    }

                    break;
                case "Effect":
                    foreach (var effectNode in subNode.Nodes)
                    {
                        var partCount = int.Parse(effectNode.Text);
                        var gearOption = new GearOption();
                        setItem.Effects[partCount] = gearOption;
                        foreach (var propNode in effectNode.Nodes)
                        {
                            switch (propNode.Text)
                            {
                                case "Option":
                                    foreach (var optionNode in propNode.Nodes)
                                    {
                                        var nodeOption = ConvertToGearOption(optionNode);
                                        gearOption.Add(nodeOption);
                                    }

                                    break;
                                default:
                                    var propType = Enum.Parse<GearPropType>(propNode.Text);
                                    var optionName = propType.GetGearOptionName();
                                    if (optionName != null)
                                    {
                                        var value = propNode.GetValue<int>();
                                        gearOption[optionName] = value;
                                    }

                                    break;
                            }
                        }
                    }

                    break;
                case "jokerPossible":
                    if (subNode.GetValue<int>() != 0)
                        setItem.JokerPossible = true;
                    break;
                case "zeroWeaponJokerPossible":
                    if (subNode.GetValue<int>() != 0)
                        setItem.ZeroWeaponJokerPossible = true;
                    break;
            }
        }

        return setItem;
    }

    private GearOption ConvertToGearOption(Wz_Node optionNode)
    {
        var optionCode = optionNode.Nodes["option"].GetValue<string>();
        var level = optionNode.Nodes["level"].GetValue<int>();
        var itemOption = itemOptionDataProvider.Data[optionCode];
        return itemOption.Level[level].Option;
    }
}