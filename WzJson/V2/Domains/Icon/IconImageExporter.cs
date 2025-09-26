using SixLabors.ImageSharp;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

public class IconImageExporter(string outputPath) : AbstractExporter<IconOrigin>
{
    private readonly string _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));

    protected override void Prepare()
    {
        Directory.CreateDirectory(_outputPath);
    }

    public override Task Export(IconOrigin iconOrigin)
    {
        return iconOrigin.Image.SaveAsPngAsync(Path.Join(_outputPath, $"{iconOrigin.Id}.png"));
    }
}