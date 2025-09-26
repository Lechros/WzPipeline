using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class TraverserNode(IPipelineNode parent, ITraverser traverser) : ITraverserNode
{
    public PipelineNodeType Type => PipelineNodeType.Traverser;
    public IPipelineNode? Parent { get; } = parent;
    public IList<IPipelineNode> Children { get; } = [];
    public ITraverser Traverser { get; } = traverser;

    public void AddChild(IConverterNode node)
    {
        Children.Add(node);
    }
}