namespace WzJson.Common.Data;

public class JsonData : IData
{
    public JsonData(string path, IDictionary<string, object>? items = null)
    {
        Path = path;
        Items = items ?? new Dictionary<string, object>();
    }

    public string Path { get; }
    public IDictionary<string, object> Items { get; }

    public void Add<T>(string key, T item) where T : notnull
    {
        Items.Add(key, item);
    }
}