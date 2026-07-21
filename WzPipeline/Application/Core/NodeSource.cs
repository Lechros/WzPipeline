using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Application.Core;

public sealed class NodeSource<T>
{
    private readonly List<ITargetBlock<T>> targets = [];
    private int terminalState;

    public void AddTarget(ITargetBlock<T> block)
    {
        targets.Add(block);
    }

    public bool HasTargets => targets.Any();

    public async Task SendAllAsync(IEnumerable<T> nodes, CancellationToken cancellationToken = default)
    {
        foreach (var item in nodes)
            await SendAsync(item, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendAsync(T item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var sends = targets.Select(target => target.SendAsync(item, cancellationToken));
        var results = await Task.WhenAll(sends).ConfigureAwait(false);
        if (results.Any(static accepted => !accepted))
        {
            var declinedTargets = targets.Where((_, index) => !results[index]).ToArray();
            try
            {
                await Task.WhenAll(declinedTargets.Select(target => target.Completion))
                    .ConfigureAwait(false);
            }
            catch
            {
                // Surface the target's parser/processor failure instead of masking it as a decline.
                throw;
            }

            throw new InvalidOperationException("One or more completed targets declined the item.");
        }
    }

    public void Complete()
    {
        if (Interlocked.CompareExchange(ref terminalState, 1, 0) != 0) return;
        foreach (var target in targets)
            target.Complete();
    }

    public void Fault(Exception exception)
    {
        if (Interlocked.CompareExchange(ref terminalState, 2, 0) != 0) return;
        foreach (var target in targets)
            target.Fault(exception);
    }

    public Task Completion => Task.WhenAll(targets.Select(target => target.Completion));
}