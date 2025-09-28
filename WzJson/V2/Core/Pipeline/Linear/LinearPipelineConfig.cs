using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class LinearPipelineConfig(RootNode node)
{
    public TraverserConfig<TNode> Traverser<TNode>(string name, ITraverser<TNode> traverser) where TNode : INode
    {
        var childNode = new TraverserNode(node, (ITraverser)traverser, name);
        node.Children.Add(childNode);
        return new TraverserConfig<TNode>(childNode);
    }
}