using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Application.Core;

public sealed class PipelineRegistry
{
    private readonly Dictionary<PipelineId, PipelineRegistration> registrations = [];

    public PipelineRegistrationBuilder<T> Register<T>(PipelineId id) where T : class, IPipeline
    {
        if (registrations.ContainsKey(id))
            throw new InvalidOperationException($"Pipeline '{id.Value}' is already registered.");
        var registration = new PipelineRegistration(id);
        registrations.Add(id, registration);
        return new PipelineRegistrationBuilder<T>(registration);
    }

    public PipelineExecutionPlan Resolve(IEnumerable<PipelineId> selectedIds)
    {
        var requested = selectedIds.Distinct().ToArray();
        var ordered = new List<PipelineRegistration>();
        var visited = new HashSet<PipelineId>();
        var visiting = new HashSet<PipelineId>();
        var path = new Stack<PipelineId>();
        foreach (var id in requested) Visit(id);
        return new PipelineExecutionPlan(requested, ordered);

        void Visit(PipelineId id)
        {
            if (visited.Contains(id)) return;
            if (!registrations.TryGetValue(id, out var registration))
                throw new InvalidOperationException($"Pipeline '{id.Value}' is not registered.");
            if (!visiting.Add(id))
            {
                var cycle = path.Reverse().SkipWhile(x => x != id).Append(id);
                throw new InvalidOperationException(
                    $"Pipeline dependency cycle detected: {string.Join(" -> ", cycle.Select(x => x.Value))}");
            }

            path.Push(id);
            foreach (var dependency in registration.Dependencies) Visit(dependency);
            path.Pop();
            visiting.Remove(id);
            visited.Add(id);
            registration.Validate();
            ordered.Add(registration);
        }
    }
}

public sealed class PipelineRegistrationBuilder<T> where T : class, IPipeline
{
    private readonly PipelineRegistration registration;

    internal PipelineRegistrationBuilder(PipelineRegistration registration)
    {
        this.registration = registration;
    }

    public PipelineRegistrationBuilder<T> DependsOn(params PipelineId[] dependencies)
    {
        registration.AddDependencies(dependencies);
        return this;
    }

    public PipelineRegistrationBuilder<T> Create(Func<PipelineFactoryContext, T> factory)
    {
        registration.SetFactory(context => factory(context));
        return this;
    }

    public PipelineRegistrationBuilder<T> Consumes<TNode>(
        SourceId sourceId, Func<T, ITargetBlock<TNode>> targetSelector)
    {
        registration.AddInput(new PipelineInputBinding<T, TNode>(sourceId, targetSelector));
        return this;
    }

    public PipelineRegistrationBuilder<T> Exports(
        Func<T, PipelineExportContext, Task> exporter)
    {
        registration.SetExporter(new PipelineExportBinding<T>(exporter));
        return this;
    }

    public PipelineRegistrationBuilder<T> Produces<TResult>(
        Func<T, Task<TResult>> resultSelector)
    {
        registration.SetResult(new PipelineResultBinding<T, TResult>(resultSelector));
        return this;
    }

    public PipelineRegistrationBuilder<T> ExportsStream<TItem>(
        Func<T, ISourceBlock<TItem>> outputSelector,
        Func<ISourceBlock<TItem>, PipelineExportContext, Task> exporter)
    {
        registration.SetStreamingExporter(
            new StreamingPipelineExportBinding<T, TItem>(outputSelector, exporter));
        return this;
    }
}

internal sealed class PipelineRegistration(PipelineId id)
{
    private Func<PipelineFactoryContext, IPipeline>? factory;
    public PipelineId Id { get; } = id;
    public List<PipelineId> Dependencies { get; } = [];
    public List<IPipelineInputBinding> Inputs { get; } = [];
    public IPipelineExportBinding? Exporter { get; private set; }
    public IPipelineResultBinding? Result { get; private set; }
    public IStreamingPipelineExportBinding? StreamingExporter { get; private set; }

    public void AddDependencies(IEnumerable<PipelineId> values)
    {
        foreach (var value in values)
        {
            if (value == Id)
                throw new InvalidOperationException($"Pipeline '{Id.Value}' cannot depend on itself.");
            if (!Dependencies.Contains(value)) Dependencies.Add(value);
        }
    }

    public void SetFactory(Func<PipelineFactoryContext, IPipeline> value)
    {
        if (factory is not null)
            throw new InvalidOperationException($"Pipeline '{Id.Value}' already has a factory.");
        factory = value;
    }

    public void AddInput(IPipelineInputBinding input)
    {
        Inputs.Add(input);
    }

    public void SetExporter(IPipelineExportBinding exporter)
    {
        if (Exporter is not null || StreamingExporter is not null)
            throw new InvalidOperationException($"Pipeline '{Id.Value}' already has an exporter.");
        Exporter = exporter;
    }

    public void SetStreamingExporter(IStreamingPipelineExportBinding exporter)
    {
        if (Exporter is not null || StreamingExporter is not null)
            throw new InvalidOperationException($"Pipeline '{Id.Value}' already has an exporter.");
        StreamingExporter = exporter;
    }

    public void SetResult(IPipelineResultBinding result)
    {
        if (Result is not null)
            throw new InvalidOperationException($"Pipeline '{Id.Value}' already has a result.");
        Result = result;
    }

    public void Validate()
    {
        if (factory is null)
            throw new InvalidOperationException($"Pipeline '{Id.Value}' does not have a factory.");
    }

    public IPipeline Create(PipelineFactoryContext context)
    {
        Validate();
        var pipeline = factory!(context);
        if (pipeline.Id != Id)
            throw new InvalidOperationException($"Factory for '{Id.Value}' created '{pipeline.Id.Value}'.");
        return pipeline;
    }
}

internal interface IPipelineExportBinding
{
    Task ExportAsync(IPipeline pipeline, PipelineExportContext context);
}

internal interface IPipelineResultBinding
{
    Task WaitAsync(IPipeline pipeline);
}

internal interface IStreamingPipelineExportBinding
{
    Task AttachAsync(IPipeline pipeline, PipelineExportContext context);
}

internal sealed class StreamingPipelineExportBinding<TPipeline, TItem>(
    Func<TPipeline, ISourceBlock<TItem>> outputSelector,
    Func<ISourceBlock<TItem>, PipelineExportContext, Task> exporter)
    : IStreamingPipelineExportBinding
    where TPipeline : class, IPipeline
{
    public Task AttachAsync(IPipeline pipeline, PipelineExportContext context)
    {
        return exporter(outputSelector((TPipeline)pipeline), context);
    }
}

internal sealed class PipelineResultBinding<TPipeline, TResult>(
    Func<TPipeline, Task<TResult>> resultSelector) : IPipelineResultBinding
    where TPipeline : class, IPipeline
{
    public Task WaitAsync(IPipeline pipeline)
    {
        return resultSelector((TPipeline)pipeline);
    }
}

internal sealed class PipelineExportBinding<T>(
    Func<T, PipelineExportContext, Task> exporter) : IPipelineExportBinding
    where T : class, IPipeline
{
    public Task ExportAsync(IPipeline pipeline, PipelineExportContext context)
    {
        return exporter((T)pipeline, context);
    }
}

internal interface IPipelineInputBinding
{
    SourceId SourceId { get; }
    void Bind(IPipeline pipeline, DispatcherExecutionSet dispatchers);
}

internal sealed class PipelineInputBinding<TPipeline, TNode>(
    SourceId sourceId,
    Func<TPipeline, ITargetBlock<TNode>> targetSelector) : IPipelineInputBinding
    where TPipeline : class, IPipeline
{
    public SourceId SourceId => sourceId;

    public void Bind(IPipeline pipeline, DispatcherExecutionSet dispatchers)
    {
        dispatchers.Bind(sourceId, pipeline.Id, targetSelector((TPipeline)pipeline));
    }
}

public sealed class PipelineExecutionPlan
{
    internal PipelineExecutionPlan(
        IReadOnlyCollection<PipelineId> requestedIds,
        IReadOnlyList<PipelineRegistration> activePipelines)
    {
        RequestedPipelineIds = requestedIds;
        ActivePipelineIds = activePipelines.Select(x => x.Id).ToArray();
        ActiveDispatcherIds = activePipelines.SelectMany(x => x.Inputs)
            .Select(x => x.SourceId).Distinct().ToArray();
        ActivePipelines = activePipelines;
    }

    public IReadOnlyCollection<PipelineId> RequestedPipelineIds { get; }
    public IReadOnlyList<PipelineId> ActivePipelineIds { get; }
    public IReadOnlyList<SourceId> ActiveDispatcherIds { get; }
    internal IReadOnlyList<PipelineRegistration> ActivePipelines { get; }

    public bool IsRequested(PipelineId id)
    {
        return RequestedPipelineIds.Contains(id);
    }
}