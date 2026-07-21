using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Core;

public sealed class SourceRegistry
{
    private readonly Dictionary<SourceId, IDispatcherRegistration> registrations = [];

    public void Register<TNode>(
        SourceId id, Func<IServiceProvider, IEnumerable<TNode>> sourceFactory)
    {
        if (!registrations.TryAdd(id, new DispatcherRegistration<TNode>(id, sourceFactory)))
            throw new InvalidOperationException($"Dispatcher '{id.Value}' is already registered.");
    }

    internal IDispatcherRegistration Get(SourceId id)
    {
        if (!registrations.TryGetValue(id, out var registration))
            throw new InvalidOperationException($"Dispatcher '{id.Value}' is not registered.");
        return registration;
    }
}

internal interface IDispatcherRegistration
{
    Type NodeType { get; }
    IDispatcherRuntime Create(IServiceProvider services);
}

internal sealed class DispatcherRegistration<TNode>(
    SourceId id,
    Func<IServiceProvider, IEnumerable<TNode>> sourceFactory) : IDispatcherRegistration
{
    public Type NodeType => typeof(TNode);

    public IDispatcherRuntime Create(IServiceProvider services)
    {
        return new DispatcherRuntime<TNode>(id, sourceFactory(services));
    }
}

internal interface IDispatcherRuntime
{
    Task RunAsync(IExecutionReporter reporter, CancellationToken cancellationToken);
    void Fault(Exception exception);
}

internal sealed class DispatcherRuntime<TNode>(
    SourceId id,
    IEnumerable<TNode> source) : IDispatcherRuntime
{
    public NodeSource<TNode> Source { get; } = new();

    public Task RunAsync(IExecutionReporter reporter, CancellationToken cancellationToken)
    {
        return NodeSourceRunner<TNode>.RunAsync(id, Source, source, reporter, cancellationToken);
    }

    public void Fault(Exception exception)
    {
        Source.Fault(exception);
    }

    public override string ToString()
    {
        return id.Value;
    }
}

internal sealed class DispatcherExecutionSet(
    SourceRegistry registry,
    IServiceProvider services)
{
    private readonly Dictionary<SourceId, IDispatcherRuntime> runtimes = [];
    private readonly Dictionary<SourceId, HashSet<PipelineId>> consumers = [];

    private readonly Dictionary<IDataflowBlock, SourceId> inputOwners =
        new(ReferenceEqualityComparer.Instance);

    public void Bind<TNode>(SourceId id, PipelineId pipelineId, ITargetBlock<TNode> target)
    {
        if (inputOwners.TryGetValue(target, out var owner) && owner != id)
            throw new InvalidOperationException(
                $"A pipeline input cannot consume both '{owner.Value}' and '{id.Value}'.");
        inputOwners[target] = id;
        if (!consumers.TryGetValue(id, out var pipelineIds))
        {
            pipelineIds = [];
            consumers.Add(id, pipelineIds);
        }
        pipelineIds.Add(pipelineId);
        GetOrCreate<TNode>(id).AddTarget(target);
    }

    public NodeSource<TNode> GetOrCreate<TNode>(SourceId id)
    {
        if (!runtimes.TryGetValue(id, out var runtime))
        {
            var registration = registry.Get(id);
            if (registration.NodeType != typeof(TNode))
                throw new InvalidOperationException(
                    $"Dispatcher '{id.Value}' carries {registration.NodeType.Name}, not {typeof(TNode).Name}.");
            runtime = registration.Create(services);
            runtimes.Add(id, runtime);
        }

        return ((DispatcherRuntime<TNode>)runtime).Source;
    }

    public Task RunAllAsync(IExecutionReporter reporter, CancellationToken cancellationToken)
    {
        var progressReporter = new PipelineInputReporter(reporter, consumers);
        return Task.WhenAll(runtimes.Values.Select(runtime =>
            runtime.RunAsync(progressReporter, cancellationToken)));
    }

    public void FaultAll(Exception exception)
    {
        foreach (var runtime in runtimes.Values) runtime.Fault(exception);
    }

    private sealed class PipelineInputReporter(
        IExecutionReporter reporter,
        IReadOnlyDictionary<SourceId, HashSet<PipelineId>> consumers) : IExecutionReporter
    {
        private readonly object sync = new();
        private readonly Dictionary<SourceId, long> sourceCounts = [];

        public void Report(ExecutionReport report)
        {
            if (report.Operation != ReportOperation.NodeSource ||
                report.Status != ReportStatus.Running || report.Current == 0)
                return;

            var sourceId = new SourceId(report.Id);
            if (!consumers.TryGetValue(sourceId, out var pipelineIds)) return;

            lock (sync)
            {
                sourceCounts[sourceId] = report.Current;
                foreach (var pipelineId in pipelineIds)
                {
                    var current = consumers
                        .Where(pair => pair.Value.Contains(pipelineId))
                        .Sum(pair => sourceCounts.GetValueOrDefault(pair.Key));
                    reporter.Report(new ExecutionReport(
                        ReportOperation.Pipeline, pipelineId.Value,
                        ReportStatus.Running, current));
                }
            }
        }
    }
}