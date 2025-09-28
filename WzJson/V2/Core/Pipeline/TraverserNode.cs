using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class TraverserNode(IPipelineNode parent, ITraverser traverser, string name) : ITraverserNode
{
    public string Name => name;
    public PipelineNodeType Type => PipelineNodeType.Traverser;
    public IPipelineNode? Parent { get; } = parent;
    public IList<IPipelineNode> Children { get; } = [];
    public ITraverser Traverser { get; } = traverser;

    public void AddChild(IConverterNode node)
    {
        Children.Add(node);
    }
}