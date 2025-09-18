using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class LinearPipelineConfig(RootNode node)
{
    public TraverserConfig<TNode> Traverser<TNode>(ITraverser<TNode> traverser) where TNode : INode
    {
        var childNode = new TraverserNode(node, (ITraverser)traverser);
        node.Children.Add(childNode);
        return new TraverserConfig<TNode>(childNode);
    }
}