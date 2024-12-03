using System.Collections;

namespace WzJson.Common.Data;

public class JsonData<TValue> : JsonData
{
    public JsonData(string path) : this(path, new Dictionary<string, TValue>())
    {
    }

    public JsonData(string path, Dictionary<string, TValue> items) : base(path, items)
    {
    }

    public new IDictionary<string, TValue> Items => (Dictionary<string, TValue>)base.Items;
}

public class JsonData : IData
{
    public JsonData(string path, IDictionary? items = null)
    {
        Path = path;
        Items = items ?? new Dictionary<string, object>();
    }

    public string Path { get; }
    public IDictionary Items { get; }

    public void Add<T>(string key, T item) where T : notnull
    {
        Items.Add(key, item);
    }
}