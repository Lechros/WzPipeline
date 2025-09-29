using System.Drawing;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

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
            Id = node.Id,
            Image = icon,
            Origin = (Point)origin
        };
    }
}