using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Core;

public sealed class PipelineFactoryContext(
    IServiceProvider services,
    IReadOnlyDictionary<PipelineId, IPipeline> pipelines,
    CancellationToken cancellationToken)
{
    public IServiceProvider Services { get; } = services;
    public CancellationToken CancellationToken { get; } = cancellationToken;

    public T GetRequiredPipeline<T>(PipelineId id) where T : class, IPipeline
    {
        if (!pipelines.TryGetValue(id, out var pipeline))
            throw new InvalidOperationException($"Pipeline '{id.Value}' has not been created.");
        return pipeline as T ?? throw new InvalidOperationException(
            $"Pipeline '{id.Value}' is not a {typeof(T).Name}.");
    }
}

public sealed class PipelineExecutionResult(
    PipelineExecutionPlan plan,
    IReadOnlyDictionary<PipelineId, IPipeline> pipelines)
{
    public PipelineExecutionPlan Plan { get; } = plan;

    public T GetRequiredPipeline<T>(PipelineId id) where T : class, IPipeline
    {
        return pipelines[id] as T ?? throw new InvalidOperationException(
            $"Pipeline '{id.Value}' is not a {typeof(T).Name}.");
    }

    internal IPipeline GetRequiredPipeline(PipelineId id)
    {
        return pipelines[id];
    }
}

public sealed class PipelineExecutor(SourceRegistry sourceRegistry)
{
    public async Task<PipelineExecutionResult> ExecuteAsync(
        PipelineExecutionPlan plan,
        IServiceProvider services,
        CancellationToken cancellationToken = default,
        IExecutionReporter? reporter = null,
        PipelineExportOptions? exportOptions = null)
    {
        reporter ??= NullExecutionReporter.Instance;
        using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var pipelines = new Dictionary<PipelineId, IPipeline>();
        var context = new PipelineFactoryContext(services, pipelines, linkedCancellation.Token);
        var dispatchers = new DispatcherExecutionSet(sourceRegistry, services);
        var completionObservers = new List<Task>();
        var resultObservers = new List<Task>();
        var streamingExportObservers = new List<Task>();

        try
        {
            foreach (var registration in plan.ActivePipelines)
                if (plan.IsRequested(registration.Id) &&
                    (registration.Exporter is not null || registration.StreamingExporter is not null))
                    reporter.Report(new ExecutionReport(
                        ReportOperation.Export, registration.Id.Value, ReportStatus.Pending));

            foreach (var registration in plan.ActivePipelines)
            {
                reporter.Report(new ExecutionReport(
                    ReportOperation.Pipeline, registration.Id.Value, ReportStatus.Running));
                var pipeline = registration.Create(context);
                pipelines.Add(registration.Id, pipeline);
                foreach (var input in registration.Inputs) input.Bind(pipeline, dispatchers);
                completionObservers.Add(ObserveCompletionAsync(registration, pipeline));
                if (registration.Result is not null)
                    resultObservers.Add(ObserveResultAsync(registration, pipeline));
            }

            foreach (var registration in plan.ActivePipelines.Where(x => x.StreamingExporter is not null))
            {
                if (!plan.IsRequested(registration.Id))
                    throw new InvalidOperationException(
                        $"Streaming pipeline '{registration.Id.Value}' must be directly requested.");
                if (exportOptions is null)
                    throw new InvalidOperationException(
                        $"Streaming pipeline '{registration.Id.Value}' requires export options.");

                var exportContext = new PipelineExportContext(
                    services, exportOptions, registration.Id, reporter, linkedCancellation.Token);
                streamingExportObservers.Add(ObserveStreamingExportAsync(
                    registration, pipelines[registration.Id], exportContext));
            }

            var executionTask = Task.WhenAll(
                dispatchers.RunAllAsync(reporter, linkedCancellation.Token),
                Task.WhenAll(completionObservers),
                Task.WhenAll(resultObservers));
            var streamingTask = Task.WhenAll(streamingExportObservers);
            var firstCompleted = await Task.WhenAny(executionTask, streamingTask).ConfigureAwait(false);
            await firstCompleted.ConfigureAwait(false);
            await Task.WhenAll(executionTask, streamingTask).ConfigureAwait(false);
            return new PipelineExecutionResult(plan, pipelines);
        }
        catch (Exception exception)
        {
            linkedCancellation.Cancel();
            dispatchers.FaultAll(exception);
            throw;
        }

        async Task ObserveCompletionAsync(PipelineRegistration registration, IPipeline pipeline)
        {
            try
            {
                await pipeline.Completion.ConfigureAwait(false);
                reporter.Report(new ExecutionReport(
                    ReportOperation.Pipeline, registration.Id.Value, ReportStatus.Completed));
            }
            catch (Exception exception)
            {
                reporter.Report(new ExecutionReport(
                    ReportOperation.Pipeline, registration.Id.Value, ReportStatus.Failed,
                    Message: exception.Message));
                throw;
            }
        }

        async Task ObserveResultAsync(PipelineRegistration registration, IPipeline pipeline)
        {
            try
            {
                await registration.Result!.WaitAsync(pipeline).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                reporter.Report(new ExecutionReport(
                    ReportOperation.Pipeline, registration.Id.Value, ReportStatus.Failed,
                    Message: exception.Message));
                throw;
            }
        }

        async Task ObserveStreamingExportAsync(
            PipelineRegistration registration,
            IPipeline pipeline,
            PipelineExportContext exportContext)
        {
            reporter.Report(new ExecutionReport(
                ReportOperation.Export, registration.Id.Value, ReportStatus.Running));
            try
            {
                await registration.StreamingExporter!
                    .AttachAsync(pipeline, exportContext).ConfigureAwait(false);
                reporter.Report(new ExecutionReport(
                    ReportOperation.Export, registration.Id.Value, ReportStatus.Completed));
            }
            catch (Exception exception)
            {
                reporter.Report(new ExecutionReport(
                    ReportOperation.Export, registration.Id.Value, ReportStatus.Failed,
                    Message: exception.Message));
                throw;
            }
        }
    }
}