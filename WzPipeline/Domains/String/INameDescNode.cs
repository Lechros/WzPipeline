using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.String;

public interface INameDescNode : INode
{
    public string? Name { get; }
    public string? Desc { get; }
}