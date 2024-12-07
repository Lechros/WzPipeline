using System.Collections;

namespace WzJson.Common.Data;

public class JsonData<TValue> : JsonData
{
    public JsonData(string label, string path) : this(label, path, new Dictionary<string, TValue>())
    {
    }

    public JsonData(string label, string path, Dictionary<string, TValue> items) : base(label, path, items)
    {
    }

    public new IDictionary<string, TValue> Items => (Dictionary<string, TValue>)base.Items;
}

public class JsonData : ILabeledData
{
    public JsonData(string label, string path, IDictionary? items = null)
    {
        Label = label;
        Path = path;
        Items = items ?? new Dictionary<string, object>();
    }

    public string Label { get; set; }
    public string Path { get; }
    public IDictionary Items { get; }

    public void Add<T>(string key, T item) where T : notnull
    {
        Items.Add(key, item);
    }
}