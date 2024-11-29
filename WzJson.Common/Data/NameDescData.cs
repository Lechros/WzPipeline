namespace WzJson.Common.Data;

public class NameDescData : IData
{
    public NameDescData(IDictionary<string, NameDesc>? items = null)
    {
        Items = items ?? new Dictionary<string, NameDesc>();
    }

    public IDictionary<string, NameDesc> Items { get; }

    public void Add<T>(string key, T item) where T : notnull
    {
        Items.Add(key, item as NameDesc);
    }
}