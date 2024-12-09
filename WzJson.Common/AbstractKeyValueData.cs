using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace WzJson.Common;

public abstract class AbstractKeyValueData<TValue> : IKeyValueData<TValue>, IKeyValueData
{
    protected readonly Dictionary<string, TValue> Dict = new();

    public IEnumerable<KeyValuePair<string, TValue>> AsEnumerable()
    {
        return this;
    }

    public virtual TValue this[string key]
    {
        get => Dict[key];
        set => Dict[key] = value;
    }

    public ICollection<string> Keys => Dict.Keys;

    public ICollection<TValue> Values => Dict.Values;

    public int Count => Dict.Count;

    public bool ContainsKey(string key)
    {
        return Dict.ContainsKey(key);
    }

    public virtual void Add(string key, TValue value)
    {
        Dict.Add(key, value);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
    {
        return Dict.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return Dict.GetEnumerator();
    }

    object? IKeyValueData.this[string key]
    {
        get => Dict[key];
        set => Dict[key] = (TValue)value;
    }

    ICollection IKeyValueData.Keys => Dict.Keys;

    ICollection IKeyValueData.Values => Dict.Values;

    bool IKeyValueData.ContainsKey(string key)
    {
        return ContainsKey(key);
    }

    void IKeyValueData.Add(string key, object? value)
    {
        Add(key, (TValue)value);
    }

    bool IKeyValueData.TryGetValue(string key, out object? value)
    {
        var ret = TryGetValue(key, out var tValue);
        value = tValue;
        return ret;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}