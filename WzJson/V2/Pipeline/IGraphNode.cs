namespace WzJson.V2.Pipeline;

public interface IGraphNode
{
    public IGraphNode? Parent { get; }
    public IList<IGraphNode> Children { get; }
}