using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.String;

public interface INameDescNode : INode
{
    public string? Name { get; }
    public string? Desc { get; }
}