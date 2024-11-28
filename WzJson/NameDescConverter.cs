using WzComparerR2.WzLib;

namespace WzJson;

public class NameDescConverter : INodeConverter<NameDesc>
{
    public NameDescData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = new NameDescData();
        foreach (var node in nodes)
        {
            var name = GetNodeName(node);
            data.Items[node.Text] = ConvertNode(node, name);
        }

        return data;
    }

    public IData NewData()
    {
        return new NameDescData();
    }

    public string GetNodeName(Wz_Node node)
    {
        return node.Text;
    }

    public NameDesc ConvertNode(Wz_Node node, string _)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new NameDesc(name, desc);
    }
}