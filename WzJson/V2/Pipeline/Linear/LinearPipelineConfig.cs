using WzJson.V2.Pipeline.Graph;
using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Linear;

public class LinearPipelineConfig(RootNode node)
{
    public TraverserConfig<TNode> Traverser<TNode>(ITraverser<TNode> traverser) where TNode : INode
    {
        var childNode = new TraverserNode(node, (ITraverser)traverser);
        node.Children.Add(childNode);
        return new TraverserConfig<TNode>(childNode);
    }
}