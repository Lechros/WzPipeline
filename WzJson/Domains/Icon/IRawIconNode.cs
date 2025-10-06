using System.Drawing;
using WzJson.Core.Stereotype;

namespace WzJson.Domains.Icon;

public interface IRawIconNode : INode
{
    public string RawIconId { get; }
    public Bitmap? RawIcon { get; }
    public Point? RawIconOrigin { get; }
}