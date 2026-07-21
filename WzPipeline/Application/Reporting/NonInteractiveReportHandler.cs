using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Reporting;

public sealed class NonInteractiveReportHandler : IReportHandler
{
    public void Report(ExecutionReport report)
    {
    }

    public Task RunAsync(
        Func<IExecutionReporter, Task> operation,
        CancellationToken cancellationToken = default)
    {
        return operation(this);
    }
}