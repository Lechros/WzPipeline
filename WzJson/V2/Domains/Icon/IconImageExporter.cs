using SixLabors.ImageSharp;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

public class IconImageExporter : AbstractExporter<IconOrigin>
{
    public override void Export(IEnumerable<IconOrigin> models, string path)
    {
        Directory.CreateDirectory(path);

        var iconOrigins = models as IconOrigin[] ?? models.ToArray();
        Parallel.ForEach(iconOrigins,
            iconOrigin => { iconOrigin.Image.SaveAsPng(Path.Join(path, $"{iconOrigin.Id}.png")); });
    }
}