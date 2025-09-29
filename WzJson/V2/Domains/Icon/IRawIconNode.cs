using System.Drawing;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

public interface IRawIconNode : INode
{
    public string RawIconId { get; }
    public Bitmap? RawIcon { get; }
    public Point? RawIconOrigin { get; }
}