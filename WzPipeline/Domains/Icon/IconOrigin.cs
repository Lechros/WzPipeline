using System.Drawing;

namespace WzPipeline.Domains.Icon;

public class IconOrigin
{
    public required string Id { get; init; }
    public required Bitmap Image { get; init; }
    public required Point Origin { get; init; }
}