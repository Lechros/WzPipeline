using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractNodeConverter<TItem> : INodeConverter<TItem>
{
    public IData Convert(IEnumerable<Wz_Node> nodes, Func<Wz_Node, string>? getNodeKey = null)
    {
        var data = NewData();
        foreach (var node in nodes)
        {
            var key = (getNodeKey ?? GetNodeKey)(node);
            var item = ConvertNode(node, key);
            if (item != null)
                data.Add(key, item);
        }

        return data;
    }

    public abstract IData NewData();

    public abstract string GetNodeKey(Wz_Node node);

    public abstract TItem? ConvertNode(Wz_Node node, string key);
}