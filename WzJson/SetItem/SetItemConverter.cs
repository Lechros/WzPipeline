using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Gear;

namespace WzJson.SetItem;

public class SetItemConverter : AbstractNodeConverter<SetItem>
{
    private readonly string dataName;
    private readonly JsonData itemOptionData;

    public SetItemConverter(string dataName, JsonData itemOptionData)
    {
        this.dataName = dataName;
        this.itemOptionData = itemOptionData;
    }

    public override IData NewData() => new JsonData(dataName);

    public override string GetNodeName(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override SetItem? ConvertNode(Wz_Node node, string _)
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
                        var props = new Dictionary<string, int>();
                        setItem.Effects[partCount] = props;
                        foreach (var propNode in effectNode.Nodes)
                        {
                            switch (propNode.Text)
                            {
                                case "Option":
                                    foreach (var optionNode in propNode.Nodes)
                                    {
                                        var option = optionNode.FindNodeByPath("option")!.GetValue<int>();
                                        var level = optionNode.FindNodeByPath("level")!.GetValue<int>();
                                        var (prop, value) = ConvertToProp(option, level);
                                        if (value != 0)
                                            props.Add(prop, value);
                                    }

                                    break;
                                default:
                                    if (Enum.TryParse(propNode.Text, out GearPropType _))
                                    {
                                        props.Add(propNode.Text, propNode.GetValue<int>());
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

    private (string, int) ConvertToProp(int option, int level)
    {
        var itemOption = (ItemOption.ItemOption)itemOptionData.Items[option.ToString()];
        var props = itemOption.Level[level];
        switch (props.Count)
        {
            case 1:
                var (prop, value) = props.First();
                return (prop, (int)value);
            case 2 when props.TryGetValue("boss", out var boss) && (int)boss != 0:
                return ("incBDR", (int)props["incDAMr"]);
            default:
                return ("", 0);
        }
    }
}