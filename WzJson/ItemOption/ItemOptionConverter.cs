using WzComparerR2.WzLib;

namespace WzJson.ItemOption;

public class ItemOptionConverter : AbstractNodeConverter<ItemOption>
{
    private readonly string dataName;

    public ItemOptionConverter(string dataName)
    {
        this.dataName = dataName;
    }

    public new JsonData Convert(IEnumerable<Wz_Node> nodes) => (JsonData)base.Convert(nodes);

    public override IData NewData() => new JsonData(dataName);

    public override string GetNodeName(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override ItemOption? ConvertNode(Wz_Node node, string _)
    {
        var infoNode = node.FindNodeByPath("info") ?? throw new InvalidDataException("info node not found");
        var levelListNode = node.FindNodeByPath("level") ?? throw new InvalidDataException("level node not found");

        var itemOption = new ItemOption();
        foreach (var subNode in infoNode.Nodes)
        {
            switch (subNode.Text)
            {
                case "optionType":
                    itemOption.OptionType = subNode.GetValue<int>();
                    break;
                case "reqLevel":
                    itemOption.ReqLevel = subNode.GetValue<int>();
                    break;
                case "string":
                    itemOption.String = subNode.GetValue<string>();
                    break;
            }
        }

        foreach (var levelNode in levelListNode.Nodes)
        {
            var level = int.Parse(levelNode.Text);
            var props = levelNode.Nodes.ToDictionary(propNode => propNode.Text, propNode => propNode.Value);
            itemOption.Level.Add(level, props);
        }

        return itemOption;
    }
}