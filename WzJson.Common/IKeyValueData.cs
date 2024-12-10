using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace WzJson.Common;

public interface IKeyValueData<TValue> : IData, IEnumerable<KeyValuePair<string, TValue>>, IKeyValueData
{
    new TValue this[string key] { get; set; }

    new ICollection<string> Keys { get; }

    new ICollection<TValue> Values { get; }

    new bool ContainsKey(string key);

    void Add(string key, TValue value);

    bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value);
}

public interface IKeyValueData : IData, IEnumerable
{
    object? this[string key] { get; set; }

    ICollection Keys { get; }

    ICollection Values { get; }

    bool ContainsKey(string key);

    void Add(string key, object? value);

    bool TryGetValue(string key, out object? value);
}