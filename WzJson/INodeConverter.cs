using WzComparerR2.WzLib;

namespace WzJson;

public interface INodeConverter<out TData> where TData : IData
{
    public TData Convert(IEnumerable<Wz_Node> nodes);
}