using Spectre.Console;
using Spectre.Console.Rendering;

namespace WzJson.Cli.Reporting;

public sealed class CountColumn : ProgressColumn
{
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var style = task.IsFinished ? new Style(Color.Green) : null;
        return new Columns(
            new Text(task.Value.ToString(), style).RightJustified(),
            new Text("/", new Style(Color.Grey)),
            new Text(task.MaxValue.ToString()).RightJustified()
        ).Padding(new Padding(0));
    }
}