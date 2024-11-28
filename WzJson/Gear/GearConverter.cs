using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Gear;

public class GearConverter : AbstractNodeConverter<Gear>
{
    private readonly string dataName;
    private readonly GlobalFindNodeFunction findNodeFunction;
    private readonly NameDescData nameDescData;

    public GearConverter(string dataName, NameDescData nameDescData, GlobalFindNodeFunction findNodeFunction)
    {
        this.dataName = dataName;
        this.findNodeFunction = findNodeFunction;
        this.nameDescData = nameDescData;
    }

    public override IData NewData()
    {
        return new JsonData(dataName);
    }

    public override string GetNodeName(Wz_Node node)
    {
        return WzUtility.GetNodeCode(node);
    }

    public override Gear? ConvertNode(Wz_Node node, string name)
    {
        nameDescData.Items.TryGetValue(name, out var nameDesc);
        if (nameDesc?.Name == null) return null;
        var infoNode = node.FindNodeByPath("info");
        if (infoNode == null) return null;
        var cashNode = infoNode.FindNodeByPath("cash");
        if (cashNode != null && cashNode.GetValue<int>() != 0) return null;

        var gear = new Gear
        {
            name = nameDesc.Name,
            desc = nameDesc.Desc,
            req = new GearReq()
        };
        foreach (var propNode in infoNode.Nodes)
        {
            switch (propNode.Text)
            {
                case "cash": break;
                case "icon":
                case "iconRaw":
                    if (gear.icon == 0)
                    {
                        var resolvedIconNode = WzUtility.ResolveLinkedNode(propNode, findNodeFunction);
                        var linkedGearNode = resolvedIconNode.ParentNode.ParentNode;
                        gear.icon = int.Parse(WzUtility.GetNodeCode(linkedGearNode));
                    }

                    break;
                case "addition": break;
                case "option":
                    gear.pots = propNode.Nodes.Select(optNode =>
                        new SpecialOption(optNode.FindNodeByPath("option")!.GetValue<int>(),
                            optNode.FindNodeByPath("level")!.GetValue<int>())).ToArray();
                    break;
                default:
                    if (!int.TryParse(propNode.Text, out _) && Enum.TryParse(propNode.Text, out GearPropType type))
                    {
                        var value = propNode.GetValue<int>();
                        switch ((int)type)
                        {
                            case < 100:
                                gear.options.Add(propNode.Text, value);
                                break;
                            case >= 1000 and < 1100:
                                switch (type)
                                {
                                    case GearPropType.reqLevel: gear.req.level = value; break;
                                    case GearPropType.reqJob: gear.req.job = value; break;
                                    case GearPropType.reqSTR: gear.req.STR = value; break;
                                    case GearPropType.reqLUK: gear.req.LUK = value; break;
                                    case GearPropType.reqDEX: gear.req.DEX = value; break;
                                    case GearPropType.reqINT: gear.req.INT = value; break;
                                    case GearPropType.reqSpecJob: gear.req.specJob = value; break;
                                }

                                break;
                            case (int)GearPropType.tuc:
                                gear.tuc = value;
                                break;
                            case (int)GearPropType.Etuc:
                                gear.etuc = value;
                                break;
                            default:
                                if (value == 0 && propNode.Nodes.Count > 0)
                                {
                                    value = 1;
                                }

                                gear.props.Add(propNode.Text, value);
                                break;
                        }
                    }

                    break;
            }
        }

        return gear;
    }
}