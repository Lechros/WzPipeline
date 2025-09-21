using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Processor;

public class DictionaryCollector<TKey, T>(Func<T, TKey> getKey)
    : AbstractProcessor<T, Dictionary<TKey, T>> where TKey : notnull
{
    public override IEnumerable<Dictionary<TKey, T>> Process(IEnumerable<T> models)
    {
        yield return models.ToDictionary(getKey, model => model);
    }
}

public static class DictionaryCollector
{
    public static DictionaryCollector<TKey, T> Create<T, TKey>(Func<T, TKey> getKey) where TKey : notnull
    {
        return new DictionaryCollector<TKey, T>(getKey);
    }
}

public class SortedDictionaryCollector<TKey, T>(Func<T, TKey> getKey)
    : AbstractProcessor<T, SortedDictionary<TKey, T>> where TKey : notnull
{
    public override IEnumerable<SortedDictionary<TKey, T>> Process(IEnumerable<T> models)
    {
        yield return new SortedDictionary<TKey, T>(models.ToDictionary(getKey, model => model));
    }
}

public static class SortedDictionaryCollector
{
    public static SortedDictionaryCollector<TKey, T> Create<T, TKey>(Func<T, TKey> getKey) where TKey : notnull
    {
        return new SortedDictionaryCollector<TKey, T>(getKey);
    }
}