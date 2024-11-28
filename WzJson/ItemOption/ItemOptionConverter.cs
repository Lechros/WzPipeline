using Newtonsoft.Json.Linq;
using WzComparerR2.WzLib;

namespace WzJson.ItemOption;

public class ItemOptionConverter : INodeConverter<IData>
{
    private readonly string dataName;

    public ItemOptionConverter(string dataName)
    {
        this.dataName = dataName;
    }

    public IData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = new JsonData(dataName);
        foreach (var node in nodes)
        {
            var name = WzUtility.GetNodeCode(node);
            var option = ConvertNode(node);
            if (option != null)
                data.Items.Add(name, option);
        }

        return data;
    }

    public ItemOption? ConvertNode(Wz_Node node)
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