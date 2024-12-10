using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractNodeConverter<TItem> : INodeConverter<TItem>, INodeConverter
{
    public abstract string GetNodeKey(Wz_Node node);

    public abstract TItem? Convert(Wz_Node node, string key);

    object? INodeConverter.Convert(Wz_Node node, string key)
    {
        return Convert(node, key);
    }
}