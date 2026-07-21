using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Core;

public sealed class PipelineExportOptions
{
    public required string OutputRootPath { get; init; }

    public IReadOnlyDictionary<PipelineId, string> OutputPaths { get; init; } =
        new Dictionary<PipelineId, string>();
}

public sealed class PipelineExportContext(
    IServiceProvider services,
    PipelineExportOptions options,
    PipelineId pipelineId,
    IExecutionReporter reporter,
    CancellationToken cancellationToken)
{
    public IServiceProvider Services { get; } = services;
    public PipelineId PipelineId { get; } = pipelineId;
    public string OutputRootPath { get; } = Path.GetFullPath(options.OutputRootPath);
    public CancellationToken CancellationToken { get; } = cancellationToken;
    public IExecutionReporter Reporter { get; } = reporter;

    public string GetOutputPath()
    {
        if (!options.OutputPaths.TryGetValue(PipelineId, out var relativePath))
            throw new InvalidOperationException(
                $"Output path for pipeline '{PipelineId.Value}' is not configured.");
        return ResolvePath(relativePath);
    }

    public string ResolvePath(string relativePath)
    {
        if (Path.IsPathRooted(relativePath))
            throw new ArgumentException("Export paths must be relative to the output root.", nameof(relativePath));

        var path = Path.GetFullPath(Path.Combine(OutputRootPath, relativePath));
        var relative = Path.GetRelativePath(OutputRootPath, path);
        if (relative == ".." || relative.StartsWith($"..{Path.DirectorySeparatorChar}"))
            throw new ArgumentException("Export path escapes the output root.", nameof(relativePath));
        return path;
    }
}

public sealed class PipelineExportRunner
{
    public async Task ExportAsync(
        PipelineExecutionResult executionResult,
        IServiceProvider services,
        PipelineExportOptions options,
        CancellationToken cancellationToken = default,
        IExecutionReporter? reporter = null)
    {
        reporter ??= NullExecutionReporter.Instance;
        foreach (var registration in executionResult.Plan.ActivePipelines)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!executionResult.Plan.IsRequested(registration.Id)) continue;
            if (registration.Exporter is null) continue;

            reporter.Report(new ExecutionReport(
                ReportOperation.Export, registration.Id.Value, ReportStatus.Running));
            var context = new PipelineExportContext(
                services, options, registration.Id, reporter, cancellationToken);
            try
            {
                await registration.Exporter.ExportAsync(
                    executionResult.GetRequiredPipeline(registration.Id), context).ConfigureAwait(false);
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