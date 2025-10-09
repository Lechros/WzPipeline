using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.Icon;

public class IconImageExporter(string outputPath) : AbstractExporter<IconOrigin>
{
    private readonly string _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));

    protected override void Prepare()
    {
        Directory.CreateDirectory(_outputPath);
    }

    public override Task Export(IconOrigin iconOrigin)
    {
        return Task.Run(() =>
        {
            iconOrigin.Image.Save(Path.Join(_outputPath, $"{iconOrigin.Id}.png"));
        });
    }

    public override void Cleanup(IconOrigin model)
    {
        model.Image.Dispose();
    }
}