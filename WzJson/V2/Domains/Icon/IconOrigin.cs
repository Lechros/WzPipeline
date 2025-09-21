using SixLabors.ImageSharp;

namespace WzJson.V2.Domains.Icon;

public class IconOrigin
{
    public required string Id { get; init; }
    public required Image Image { get; init; }
    public required Point Origin { get; init; }
}