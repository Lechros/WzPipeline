using WzComparerR2.WzLib;

namespace WzJson.Common;

public interface INodeConverter<out T> : INodeConverter
{
    public new T? Convert(Wz_Node node, string key);
}

public interface INodeConverter
{
    public string GetNodeKey(Wz_Node node);
    
    public object? Convert(Wz_Node node, string key);
}