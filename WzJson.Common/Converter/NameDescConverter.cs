using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class NameDescConverter : AbstractNodeConverter<NameDesc>
{
    public static NameDescConverter Instance { get; } = new();
    
    public new JsonData<NameDesc> Convert(IEnumerable<Wz_Node> nodes)
    {
        return (JsonData<NameDesc>)base.Convert(nodes);
    }

    public JsonData<NameDesc> Convert(IEnumerable<Wz_Node> nodes, Func<Wz_Node, string> getNodeKey)
    {
        var data = (JsonData<NameDesc>)NewData();
        foreach (var node in nodes)
        {
            var key = getNodeKey(node);
            var item = ConvertNode(node, key);
            if (item != null)
                data.Add(key, item);
        }

        return data;
    }

    public override IData NewData() => new JsonData<NameDesc>("");

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override NameDesc ConvertNode(Wz_Node node, string _)
    {
        var name = node.FindNodeByPath("name")?.GetValue<string>();
        var desc = node.FindNodeByPath("desc")?.GetValue<string>();
        return new NameDesc(name, desc);
    }
}