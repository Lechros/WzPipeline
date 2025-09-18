namespace WzJson.V2.Core.Pipeline;

public interface IGraphNode
{
    public IGraphNode? Parent { get; }
    public IList<IGraphNode> Children { get; }
}