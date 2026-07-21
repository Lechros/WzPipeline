using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Core;

public sealed class NodeSourceRunner<T>(NodeSource<T> source)
{
    public static Task RunAsync(
        SourceId id,
        NodeSource<T> source,
        IEnumerable<T> nodes,
        IExecutionReporter reporter,
        CancellationToken cancellationToken = default)
    {
        return new NodeSourceRunner<T>(source)
            .RunAsync(id, nodes, reporter, cancellationToken);
    }

    public async Task RunAsync(
        SourceId id,
        IEnumerable<T> nodes,
        IExecutionReporter reporter,
        CancellationToken cancellationToken = default)
    {
        if (!source.HasTargets) return;

        long count = 0;
        reporter.Report(new ExecutionReport(
            ReportOperation.NodeSource, id.Value, ReportStatus.Running));
        try
        {
            foreach (var node in nodes)
            {
                await source.SendAsync(node, cancellationToken).ConfigureAwait(false);
                count++;
                reporter.Report(new ExecutionReport(
                    ReportOperation.NodeSource, id.Value, ReportStatus.Running, count));
            }

            source.Complete();
        }
        catch (Exception ex)
        {
            source.Fault(ex);
            reporter.Report(new ExecutionReport(
                ReportOperation.NodeSource, id.Value, ReportStatus.Failed, count,
                ex.Message));
            throw;
        }

        await source.Completion.ConfigureAwait(false);
        reporter.Report(new ExecutionReport(
            ReportOperation.NodeSource, id.Value, ReportStatus.Completed, count));
    }
}