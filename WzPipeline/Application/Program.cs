using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using WzPipeline.Application.Configuration;
using WzPipeline.Application.Core;
using WzPipeline.Application.Core.Reporting;
using WzPipeline.Application.Reporting;

namespace WzPipeline.Application;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var cancellation = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellation.Cancel();
        };

        var selectedIds = PipelineSelection.Select(args);
        if (selectedIds.Count == 0) return;

        var services = new ServiceCollection();
        services.AddApplicationServices();
        await using var provider = services.BuildServiceProvider();

        var pipelineRegistry = PipelineRegistryFactory.Create();
        var dispatcherRegistry = SourceRegistryFactory.Create();
        var plan = pipelineRegistry.Resolve(selectedIds);

        AnsiConsole.MarkupLine(
            $"Executing: [blue]{Markup.Escape(string.Join(", ", plan.ActivePipelineIds.Select(x => x.Value)))}[/]");
        var exportOptions = ApplicationConfiguration.CreateExportOptions();
        IReportHandler reportHandler = Console.IsOutputRedirected
            ? new NonInteractiveReportHandler()
            : new ConsoleLiveReportHandler();
        var executionStopwatch = Stopwatch.StartNew();
        await reportHandler.RunAsync(async reporter =>
        {
            var executionResult = await new PipelineExecutor(dispatcherRegistry)
                .ExecuteAsync(plan, provider, cancellation.Token, reporter, exportOptions);
            await new PipelineExportRunner().ExportAsync(
                executionResult, provider, exportOptions, cancellation.Token, reporter);
        }, cancellation.Token);
        executionStopwatch.Stop();
        AnsiConsole.MarkupLine("[green]All operations completed successfully![/]");
        AnsiConsole.MarkupLineInterpolated($"Duration: [blue]{FormatDuration(executionStopwatch.Elapsed)}[/]");
        AnsiConsole.Write("Output path: ");
        AnsiConsole.Write(new TextPath(exportOptions.OutputRootPath));
        AnsiConsole.WriteLine();
    }

    private static string FormatDuration(TimeSpan duration)
    {
        return duration.ToString(@"hh\:mm\:ss\.fff");
    }
}