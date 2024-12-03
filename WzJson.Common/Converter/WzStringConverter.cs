using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class WzStringConverter : AbstractNodeConverter<WzString>
{
    public static WzStringConverter Instance { get; } = new();

    public new WzStringData Convert(IEnumerable<Wz_Node> nodes)
    {
        return (WzStringData)base.Convert(nodes);
    }

    public WzStringData Convert(IEnumerable<Wz_Node> nodes, Func<Wz_Node, string> getNodeKey)
    {
        var data = (WzStringData)NewData();
        foreach (var node in nodes)
        {
            var key = getNodeKey(node);
            var item = ConvertNode(node, key);
            if (item != null)
                data.Add(key, item);
        }

        return data;
    }

    public override IData NewData() => new WzStringData();

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override WzString ConvertNode(Wz_Node node, string _)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new WzString
        {
            Name = name,
            Desc = desc
        };
    }
}