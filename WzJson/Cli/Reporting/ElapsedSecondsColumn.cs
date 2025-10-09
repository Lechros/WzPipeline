using Spectre.Console;
using Spectre.Console.Rendering;

namespace WzJson.Cli.Reporting;

public sealed class ElapsedSecondsColumn : ProgressColumn
{
    protected override bool NoWrap => true;

    public Style Style { get; set; } = Color.Blue;

    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var elapsed = task.ElapsedTime;
        if (elapsed == null)
        {
            return new Markup("-s");
        }

        if (elapsed.Value.TotalSeconds >= 100)
        {
            return new Text($"{elapsed.Value:m\\:ss}", Style).RightJustified();
        }

        if (elapsed.Value.TotalSeconds >= 10)
        {
            return new Text($"{elapsed.Value.TotalSeconds:0.0}s", Style);
        }

        return new Text($"{elapsed.Value.TotalSeconds:0.00}s", Style);
    }

    public override int? GetColumnWidth(RenderOptions options)
    {
        return 5;
    }
}