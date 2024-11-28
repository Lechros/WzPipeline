using WzComparerR2.WzLib;

namespace WzJson;

public interface IDataConverter<out TData> where TData : IData
{
    public TData Convert(IEnumerable<Wz_Node> node);
}