using WzJson.Common;

namespace WzJson.DataProvider;

public interface IDataProvider<out TData> where TData : IData
{
    public TData Data { get; }
}