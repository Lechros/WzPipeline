using WzComparerR2.WzLib;

namespace WzJson;

public interface INodeConverter<out T>
{
    public IData NewData();
    
    public string GetNodeName(Wz_Node node);

    public T? ConvertNode(Wz_Node node, string name);
}