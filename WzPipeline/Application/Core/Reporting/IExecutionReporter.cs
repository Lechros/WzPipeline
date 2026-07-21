namespace WzPipeline.Application.Core.Reporting;

public interface IExecutionReporter
{
    void Report(ExecutionReport report);
}

public sealed class NullExecutionReporter : IExecutionReporter
{
    public static readonly NullExecutionReporter Instance = new();

    private NullExecutionReporter()
    {
    }

    public void Report(ExecutionReport report)
    {
    }
}

public interface IReportHandler : IExecutionReporter
{
    Task RunAsync(
        Func<IExecutionReporter, Task> operation,
        CancellationToken cancellationToken = default);
}