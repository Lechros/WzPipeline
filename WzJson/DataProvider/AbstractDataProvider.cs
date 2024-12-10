using WzJson.Common;

namespace WzJson.DataProvider;

public abstract class AbstractDataProvider<TData> : IDataProvider<TData> where TData : IData
{
    private readonly Lazy<TData> lazyData;

    protected AbstractDataProvider()
    {
        lazyData = new Lazy<TData>(GetData);
    }

    public TData Data => lazyData.Value;

    protected abstract TData GetData();
}