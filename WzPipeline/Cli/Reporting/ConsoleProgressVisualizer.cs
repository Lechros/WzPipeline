using Spectre.Console;
using WzPipeline.Core.Pipeline.Runner;

namespace WzPipeline.Cli.Reporting;

public class ConsoleProgressVisualizer(ProgressContext ctx)
{
    private ProgressTask[]? _tasks;

    public void Update(IStepState rootState)
    {
        var states = Dfs(rootState).Skip(1).ToArray();
        if (_tasks == null)
        {
            _tasks = states.Select(e => ctx.AddTask(GetName(e.state, e.depth), autoStart: false)).ToArray();
        }
        else
        {
            if (_tasks.Length != states.Length)
            {
                throw new ArgumentException("Different length");
            }
        }

        foreach (var (task, state) in _tasks.Zip(states.Select(e => e.state)))
        {
            if (state.TotalCount.HasValue) task.MaxValue = state.TotalCount.Value;
            task.Value = state.Count;
            if (!task.IsStarted && state.Status != NodeStatus.Pending) task.StartTask();
            if (task.StopTime == null && state.Status == NodeStatus.Complete) task.StopTask();
        }
    }

    private static string GetName(IStepState state, int depth)
    {
        depth--;
        return depth == 0
            ? state.Name
            : $"\0{new string(' ', (depth - 1) * 2)} └ {state.Name}"; // \0 to stop Trim()
    }

    private static IEnumerable<(IStepState state, int depth)> Dfs(IStepState state, int depth = 0)
    {
        yield return (state, depth);
        foreach (var child in state.Children)
        {
            foreach (var descendants in Dfs(child, depth + 1))
            {
                yield return descendants;
            }
        }
    }
}

internal static class ConsoleProgressVisualizerExtensions
{
    public static Progress ConfigureConsoleProgressVisualizerColumns(this Progress progress)
    {
        return progress.Columns(
            new TaskDescriptionColumn() { Alignment = Justify.Left },
            new ProgressBarColumn(),
            new PercentageColumn(),
            new CountColumn(),
            new ElapsedSecondsColumn(),
            new SpinnerColumn()
        );
    }
}