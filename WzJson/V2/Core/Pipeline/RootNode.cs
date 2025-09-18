namespace WzJson.V2.Core.Pipeline;

public class RootNode : IGraphNode
{
    public IGraphNode? Parent => null;
    public IList<IGraphNode> Children { get; } = [];
}