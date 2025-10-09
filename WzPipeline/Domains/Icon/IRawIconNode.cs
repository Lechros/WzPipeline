using System.Drawing;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.Icon;

public interface IRawIconNode : INode
{
    public string RawIconId { get; }
    public Bitmap? RawIcon { get; }
    public Point? RawIconOrigin { get; }
}