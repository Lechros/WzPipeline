using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Application.Shared.Dataflow;

public static class DataflowCollectors
{
    public static ITargetBlock<T> DictionaryCollector<T, TKey>(IDictionary<TKey, T> dictionary,
        Func<T, TKey> keySelector)
    {
        return new ActionBlock<T>(e => dictionary.Add(keySelector(e), e),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
    }

    public static ITargetBlock<T> DictionaryCollector<T, TKey, TElement>(IDictionary<TKey, TElement> dictionary,
        Func<T, TKey> keySelector, Func<T, TElement> elementSelector)
    {
        return new ActionBlock<T>(e => dictionary.Add(keySelector(e), elementSelector(e)),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
    }

    public static ITargetBlock<KeyValuePair<TKey, TValue>> DictionaryCollector<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary)
    {
        return new ActionBlock<KeyValuePair<TKey, TValue>>(e => dictionary.Add(e.Key, e.Value),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
    }
}