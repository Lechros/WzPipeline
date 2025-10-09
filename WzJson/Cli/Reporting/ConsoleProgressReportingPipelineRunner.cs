using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Rendering;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;

namespace WzJson.Cli.Reporting;

class ConsoleProgressReportingPipelineRunner : IPipelineRunner
{
    private readonly DefaultPipelineRunner _runner = new();

    public IStepState Run(PipelineRoot root, IProgress<IStepState>? progress = null)
    {
        IStepState? result = null;

        var consoleProgress = AnsiConsole.Progress()
            .ConfigureConsoleProgressVisualizerColumns();
        consoleProgress.RenderHook = CreateRenderHook(root.Name, DateTime.Now);
        consoleProgress.Start(ctx =>
        {
            var visualizer = new ConsoleProgressVisualizer(ctx);
            progress = new Progress<IStepState>(visualizer.Update);

            result = _runner.Run(root, progress);

            visualizer.Update(result);
            ctx.Refresh();
        });
        return result!;
    }

    private static Func<IRenderable, IReadOnlyList<ProgressTask>, IRenderable> CreateRenderHook(string name,
        DateTime start)
    {
        return (renderable, tasks) =>
        {
            var header = new Panel(name);
            var footer = new Markup($"Duration: [blue]{(DateTime.Now - start).TotalSeconds:0.00}s[/]");
            return new Rows(header, renderable, footer);
        };
    }
}