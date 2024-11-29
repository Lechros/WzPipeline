using WzComparerR2.WzLib;

namespace WzJson;

public class NameDescConverter : AbstractNodeConverter<NameDesc>
{
    public new NameDescData Convert(IEnumerable<Wz_Node> nodes)
    {
        return (NameDescData)base.Convert(nodes);
    }

    public override IData NewData() => new NameDescData();

    public override string GetNodeName(Wz_Node node) => node.Text;

    public override NameDesc ConvertNode(Wz_Node node, string _)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new NameDesc(name, desc);
    }
}