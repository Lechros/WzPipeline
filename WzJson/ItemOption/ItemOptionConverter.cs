using Newtonsoft.Json.Linq;
using WzComparerR2.WzLib;

namespace WzJson.ItemOption;

public class ItemOptionConverter : AbstractNodeConverter<ItemOption>
{
    private readonly string dataName;

    public ItemOptionConverter(string dataName)
    {
        this.dataName = dataName;
    }

    public new JsonData Convert(IEnumerable<Wz_Node> nodes)
    {
        return (JsonData)base.Convert(nodes);
    }

    public override IData NewData()
    {
        return new JsonData(dataName);
    }

    public override string GetNodeName(Wz_Node node)
    {
        return WzUtility.GetNodeCode(node);
    }

    public override ItemOption? ConvertNode(Wz_Node node, string _)
    {
        var infoNode = node.FindNodeByPath("info") ?? throw new InvalidDataException("info node not found");
        var levelNode = node.FindNodeByPath("level") ?? throw new InvalidDataException("level node not found");

        var option = new ItemOption();
        foreach (var subNode in infoNode.Nodes)
        {
            switch (subNode.Text)
            {
                case "optionType":
                    option.optionType = subNode.GetValue<int>();
                    break;
                case "reqLevel":
                    option.reqLevel = subNode.GetValue<int>();
                    break;
                case "string":
                    option.@string = subNode.GetValue<string>();
                    break;
            }
        }

        foreach (var optionNode in levelNode.Nodes)
        {
            foreach (var subNode in optionNode.Nodes)
            {
                option.addOption(optionNode.Text, subNode.Text, new JValue(subNode.Value));
            }
        }

        return option;
    }
}