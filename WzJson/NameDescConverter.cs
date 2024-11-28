using WzComparerR2.WzLib;

namespace WzJson;

public class NameDescConverter : IDataConverter<NameDescData>
{
    public NameDescData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = new NameDescData();
        foreach (var node in nodes)
        {
            data.Items[node.Text] = ConvertNode(node);
        }

        return data;
    }

    public NameDesc ConvertNode(Wz_Node node)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new NameDesc(name, desc);
    }
}