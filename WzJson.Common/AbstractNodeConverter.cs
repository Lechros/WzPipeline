using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractNodeConverter<T> : INodeConverter<T>
{
    public abstract IData NewData();

    public abstract string GetNodeKey(Wz_Node node);

    public abstract T? ConvertNode(Wz_Node node, string key);

    public IData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = NewData();
        foreach (var node in nodes)
        {
            var key = GetNodeKey(node);
            var item = ConvertNode(node, key);
            if (item != null)
                data.Add(key, item);
        }

        return data;
    }
}