using System.Drawing;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

public interface IIconNode : INode
{
    public string IconId { get; }
    public Bitmap? Icon { get; }
    public Point? IconOrigin { get; }
}