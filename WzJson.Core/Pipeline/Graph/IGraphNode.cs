namespace WzJson.Core.Pipeline.Graph;

public interface IGraphNode
{
    public IGraphNode? Parent { get; }
    public IList<IGraphNode> Children { get; }
}