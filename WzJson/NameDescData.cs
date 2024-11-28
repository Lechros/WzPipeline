namespace WzJson;

public class NameDescData : IData
{
    public NameDescData(IDictionary<string, NameDesc>? items = null)
    {
        Items = items ?? new Dictionary<string, NameDesc>();
    }

    public IDictionary<string, NameDesc> Items { get; }
}