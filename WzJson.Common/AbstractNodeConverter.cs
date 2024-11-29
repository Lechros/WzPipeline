using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractNodeConverter<T> : INodeConverter<T>
{
    public abstract IData NewData();

    public abstract string GetNodeName(Wz_Node node);

    public abstract T? ConvertNode(Wz_Node node, string name);

    public IData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = NewData();
        foreach (var node in nodes)
        {
            var name = GetNodeName(node);
            var item = ConvertNode(node, name);
            if (item != null)
                data.Add(name, item);
        }

        return data;
    }
}