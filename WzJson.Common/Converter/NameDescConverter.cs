using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class NameDescConverter : AbstractNodeConverter<NameDesc>
{
    public static NameDescConverter Instance { get; } = new();
    
    public new NameDescData Convert(IEnumerable<Wz_Node> nodes)
    {
        return (NameDescData)base.Convert(nodes);
    }

    public NameDescData Convert(IEnumerable<Wz_Node> nodes, Func<Wz_Node, string> getNodeKey)
    {
        var data = (NameDescData)NewData();
        foreach (var node in nodes)
        {
            var key = getNodeKey(node);
            var item = ConvertNode(node, key);
            if (item != null)
                data.Add(key, item);
        }

        return data;
    }

    public override IData NewData() => new NameDescData();

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override NameDesc ConvertNode(Wz_Node node, string _)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new NameDesc(name, desc);
    }
}