namespace WzPipeline.Application.Core.Reporting;

public enum ReportOperation
{
    NodeSource,
    Pipeline,
    Export
}

public enum ReportStatus
{
    Pending,
    Running,
    Completed,
    Failed,
    Cancelled
}

public readonly record struct ExecutionReport(
    ReportOperation Operation,
    string Id,
    ReportStatus Status,
    long Current = 0,
    string? Message = null);