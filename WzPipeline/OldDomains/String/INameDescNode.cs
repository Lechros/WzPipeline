using WzPipeline.Core.Stereotype;

namespace WzPipeline.OldDomains.String;

public interface INameDescNode : INode
{
    public string? Name { get; }
    public string? Desc { get; }
}