using System.Drawing;
using WzJson.Core.Stereotype;

namespace WzJson.Domains.Icon;

public class RawIconConverter : AbstractConverter<IRawIconNode, IconOrigin>
{
    public override IconOrigin? Convert(IRawIconNode node)
    {
        var origin = node.RawIconOrigin;
        if (!origin.HasValue)
            return null;
        var icon = node.RawIcon;
        if (icon == null)
            return null;
        return new IconOrigin
        {
            Id = node.RawIconId,
            Image = icon,
            Origin = (Point)origin
        };
    }
}