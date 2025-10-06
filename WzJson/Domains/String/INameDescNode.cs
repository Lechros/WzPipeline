using WzJson.Core.Stereotype;

namespace WzJson.Domains.String;

public interface INameDescNode : INode
{
    public string? Name { get; }
    public string? Desc { get; }
}