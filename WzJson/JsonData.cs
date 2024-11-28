namespace WzJson;

public class JsonData : IData
{
    public JsonData(string path, IDictionary<string, object>? items = null)
    {
        Path = path;
        Items = items ?? new Dictionary<string, object>();
    }

    public string Path { get; }
    public IDictionary<string, object> Items { get; }
}