namespace WzJson;

public class JsonData<T> : IData<T>
{
    public JsonData(string name, IDictionary<string, T>? items = null)
    {
        Name = name;
        Items = items ?? new Dictionary<string, T>();
    }

    public string Name { get; }
    public IDictionary<string, T> Items { get; }
}