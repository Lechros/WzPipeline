namespace WzJson.Common.V2.Graph;

public interface IGraphNode
{
    public IGraphNode? Parent { get; }
    public IList<IGraphNode> Children { get; }
}