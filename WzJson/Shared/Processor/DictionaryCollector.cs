using WzJson.Core.Stereotype;

namespace WzJson.Shared.Processor;

public static class DictionaryCollector
{
    public static DictionaryCollector<TSource, TKey, TSource, IDictionary<TKey, TSource>> Create<TSource, TKey>(
        Func<TSource, TKey> keySelector) where TKey : notnull
    {
        return Create(keySelector, DictionaryFactory<TKey, TSource>);
    }

    public static DictionaryCollector<TSource, TKey, TValue, IDictionary<TKey, TValue>>
        Create<TSource, TKey, TValue>(Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) where TKey : notnull
    {
        return Create(keySelector, elementSelector, DictionaryFactory<TKey, TValue>);
    }

    public static DictionaryCollector<TSource, TKey, TSource, TDictionary> Create<TSource, TKey, TDictionary>(
        Func<TSource, TKey> keySelector, Func<TDictionary> dictionaryFactory)
        where TDictionary : IDictionary<TKey, TSource>
    {
        return new DictionaryCollector<TSource, TKey, TSource, TDictionary>(keySelector, s => s, dictionaryFactory);
    }

    public static DictionaryCollector<TSource, TKey, TValue, TDictionary> Create<TSource, TKey, TValue, TDictionary>(
        Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, Func<TDictionary> dictionaryFactory)
        where TDictionary : IDictionary<TKey, TValue>
    {
        return new DictionaryCollector<TSource, TKey, TValue, TDictionary>(keySelector, elementSelector,
            dictionaryFactory);
    }

    private static IDictionary<TKey, TValue> DictionaryFactory<TKey, TValue>() where TKey : notnull
    {
        return new Dictionary<TKey, TValue>();
    }
}

public class DictionaryCollector<TSource, TKey, TValue, TDictionary> : AbstractProcessor<TSource, TDictionary>
    where TDictionary : IDictionary<TKey, TValue>
{
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TValue> _elementSelector;
    private readonly Func<TDictionary> _dictionaryFactory;

    internal DictionaryCollector(Func<TSource, TKey> keySelector,
        Func<TSource, TValue> elementSelector, Func<TDictionary> dictionaryFactory)
    {
        _keySelector = keySelector;
        _elementSelector = elementSelector;
        _dictionaryFactory = dictionaryFactory;
    }

    public override IEnumerable<TDictionary> Process(IEnumerable<TSource> models)
    {
        var result = _dictionaryFactory();
        foreach (var model in models)
        {
            result.Add(_keySelector(model), _elementSelector(model));
        }

        yield return result;
    }
}