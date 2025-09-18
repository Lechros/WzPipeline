using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline;

public class TraverserNode(IGraphNode parent, ITraverser traverser) : ITraverserNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
    public ITraverser Traverser { get; } = traverser;

    public void AddChild(IConverterNode node)
    {
        Children.Add(node);
    }
}