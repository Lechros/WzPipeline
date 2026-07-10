using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Shared;

public static class DataflowExtensions
{
    public static ISourceBlock<T> ToSourceBlock<T>(this IEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
        var block = new BufferBlock<T>();
        _ = Task.Run(async () =>
        {
            try
            {
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await block.SendAsync(item, cancellationToken);
                }

                block.Complete();
            }
            catch (Exception ex)
            {
                ((IDataflowBlock)block).Fault(ex);
            }
        }, cancellationToken);
        return block;
    }

    public static IReceivableSourceBlock<TOutput> Map<TInput, TOutput>(this ISourceBlock<TInput> source,
        Func<TInput, TOutput> selector, ExecutionDataflowBlockOptions? options = null)
    {
        var transform = options == null
            ? new TransformBlock<TInput, TOutput>(selector)
            : new TransformBlock<TInput, TOutput>(selector, options);
        source.LinkTo(transform, new DataflowLinkOptions { PropagateCompletion = true });
        return transform;
    }
}