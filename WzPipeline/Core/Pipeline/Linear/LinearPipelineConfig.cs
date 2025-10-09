using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline.Linear;

public class LinearPipelineConfig(PipelineRoot node)
{
    public TraverserConfig<TNode> Traverser<TNode>(string name, ITraverser<TNode> traverser) where TNode : INode
    {
        var childNode = new TraverserStep(node, (ITraverser)traverser, name);
        node.Children.Add(childNode);
        return new TraverserConfig<TNode>(childNode);
    }
}