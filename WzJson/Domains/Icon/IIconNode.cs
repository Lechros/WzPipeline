using System.Drawing;
using WzJson.Core.Stereotype;

namespace WzJson.Domains.Icon;

public interface IIconNode : INode
{
    public string IconId { get; }
    public Bitmap? Icon { get; }
    public Point? IconOrigin { get; }
}