using System.Drawing;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.Icon;

public interface IIconNode : INode
{
    public string IconId { get; }
    public Bitmap? Icon { get; }
    public Point? IconOrigin { get; }
}