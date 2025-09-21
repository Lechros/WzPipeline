using SixLabors.ImageSharp;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Icon;

public interface IRawIconNode : INode
{
    public string RawIconId { get; }
    public Image? RawIcon { get; }
    public Point? RawIconOrigin { get; }
}