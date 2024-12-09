using WzComparerR2.WzLib;

namespace WzJson.Common;

public interface INodeConverter<out T>
{
    public IKeyValueData NewData();

    public string GetNodeKey(Wz_Node node);

    public T? ConvertNode(Wz_Node node, string key);
}