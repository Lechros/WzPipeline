using System.Diagnostics;
using System.Globalization;
using Spectre.Console;
using WzPipeline.Application.Core.Reporting;

namespace WzPipeline.Application.Reporting;

public sealed class ConsoleLiveReportHandler : IReportHandler
{
    private readonly object sync = new();
    private readonly Dictionary<(ReportOperation Operation, string Id), ReportState> states = [];
    private readonly TimeSpan refreshInterval;
    private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

    public ConsoleLiveReportHandler(TimeSpan? refreshInterval = null)
    {
        this.refreshInterval = refreshInterval ?? TimeSpan.FromMilliseconds(100);
    }

    public void Report(ExecutionReport report)
    {
        lock (sync)
        {
            var key = (report.Operation, report.Id);
            if (states.TryGetValue(key, out var previous))
            {
                var merged = report with
                {
                    Current = report.Status != ReportStatus.Running &&
                              report.Current == 0 && previous.Report.Current > 0
                        ? previous.Report.Current
                        : report.Current,
                    Status = IsTerminal(previous.Report.Status) &&
                             report.Status == ReportStatus.Running
                        ? previous.Report.Status
                        : report.Status
                };
                var isStarting = previous.Report.Status != ReportStatus.Running &&
                                 merged.Status == ReportStatus.Running;
                var startedTicks = isStarting ? Stopwatch.ElapsedTicks : previous.StartedTicks;
                states[key] = new ReportState(
                    merged,
                    startedTicks,
                    merged.Status == ReportStatus.Running ? null : previous.EndedTicks ?? Stopwatch.ElapsedTicks);
            }
            else
            {
                var now = Stopwatch.ElapsedTicks;
                states[key] = new ReportState(
                    report, now, report.Status == ReportStatus.Running ? null : now);
            }
        }
    }

    public Task RunAsync(
        Func<IExecutionReporter, Task> operation,
        CancellationToken cancellationToken = default)
    {
        return AnsiConsole.Live(CreateTable()).StartAsync(async context =>
        {
            // The orchestration contains synchronous WZ loading and Dataflow sends that can
            // complete synchronously. Run it away from the Live render loop so the first
            // incomplete await is not required before the UI can start refreshing.
            var operationTask = Task.Run(() => operation(this));
            while (!operationTask.IsCompleted)
            {
                await Task.WhenAny(
                    operationTask,
                    Task.Delay(refreshInterval)).ConfigureAwait(false);
                context.UpdateTarget(CreateTable());
                context.Refresh();
            }

            try
            {
                await operationTask.ConfigureAwait(false);
            }
            finally
            {
                context.UpdateTarget(CreateTable());
                context.Refresh();
            }
        });
    }

    private Table CreateTable()
    {
        ReportState[] snapshot;
        lock (sync)
        {
            snapshot = states.Values
                .OrderBy(state => state.Report.Operation)
                .ThenBy(state => state.Report.Id, StringComparer.Ordinal)
                .ToArray();
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Operation")
            .AddColumn("Name")
            .AddColumn(new TableColumn("Count").RightAligned())
            .AddColumn("Status")
            .AddColumn(new TableColumn("Duration").RightAligned());

        foreach (var state in snapshot)
        {
            var report = state.Report;
            table.AddRow(
                new Text(report.Operation.ToString()),
                new Text(report.Id),
                new Text(report.Current.ToString()),
                new Markup(FormatStatus(report)),
                new Markup($"[blue]{FormatDuration(state)}[/]"));
        }

        return table;
    }

    private static string FormatDuration(ReportState state)
    {
        var endedTicks = state.EndedTicks ?? Stopwatch.ElapsedTicks;
        var elapsed = TimeSpan.FromSeconds(
            (endedTicks - state.StartedTicks) / (double)Stopwatch.Frequency);
        return string.Create(CultureInfo.InvariantCulture, $"{elapsed.TotalSeconds:0.0}s");
    }

    private static string FormatStatus(ExecutionReport report)
    {
        var color = report.Status switch
        {
            ReportStatus.Pending => "grey",
            ReportStatus.Running => "yellow",
            ReportStatus.Completed => "green",
            ReportStatus.Failed => "red",
            ReportStatus.Cancelled => "grey",
            _ => "white"
        };
        var text = Markup.Escape(report.Message ?? report.Status.ToString());
        return $"[{color}]{text}[/]";
    }

    private static bool IsTerminal(ReportStatus status)
    {
        return status is ReportStatus.Completed or ReportStatus.Failed or ReportStatus.Cancelled;
    }

    private sealed record ReportState(
        ExecutionReport Report,
        long StartedTicks,
        long? EndedTicks);
}