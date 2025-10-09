using System.Drawing;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.Icon;

public class IconConverter : AbstractConverter<IIconNode, IconOrigin>
{
    public override IconOrigin? Convert(IIconNode node)
    {
        var origin = node.IconOrigin;
        if (!origin.HasValue)
            return null;
        var icon = node.Icon;
        if (icon == null)
            return null;
        return new IconOrigin
        {
            Id = node.IconId,
            Image = icon,
            Origin = (Point)origin
        };
    }
}