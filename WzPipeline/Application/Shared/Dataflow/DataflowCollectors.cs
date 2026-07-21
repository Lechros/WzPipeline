using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Application.Shared.Dataflow;

public static class DataflowCollectors
{
    public static ITargetBlock<T> DictionaryCollector<T, TKey>(IDictionary<TKey, T> dictionary,
        Func<T, TKey> keySelector, CancellationToken cancellationToken = default)
    {
        return new ActionBlock<T>(e => dictionary.Add(keySelector(e), e),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = 1 });
    }

    public static ITargetBlock<T> DictionaryCollector<T, TKey, TElement>(IDictionary<TKey, TElement> dictionary,
        Func<T, TKey> keySelector, Func<T, TElement> elementSelector, CancellationToken cancellationToken = default)
    {
        return new ActionBlock<T>(e => dictionary.Add(keySelector(e), elementSelector(e)),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = 1 });
    }

    public static ITargetBlock<KeyValuePair<TKey, TValue>> DictionaryCollector<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary, CancellationToken cancellationToken = default)
    {
        return new ActionBlock<KeyValuePair<TKey, TValue>>(e => dictionary.Add(e.Key, e.Value),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken, MaxDegreeOfParallelism = 1 });
    }
}