namespace WzJson.Common;

public abstract class AbstractDictionaryData<TItem>(IDictionary<string, TItem> items) : IData
{
    public IDictionary<string, TItem> Items { get; } = items;

    public void Add(string key, TItem item)
    {
        Items.Add(key, item);
    }

    void IData.Add(string key, dynamic item)
    {
        Add(key, (TItem)item);
    }
}