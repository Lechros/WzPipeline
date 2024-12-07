using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class WzStringConverter : AbstractNodeConverter<WzString>
{
    public new WzStringData Convert(IEnumerable<Wz_Node> nodes, Func<Wz_Node, string>? getNodeKey = null)
    {
        return (WzStringData)base.Convert(nodes, getNodeKey);
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