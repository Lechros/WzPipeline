using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline.Graph;

public class GraphPipelineConfig(PipelineRoot node)
{
    public GraphPipelineConfig Traverser<TNode>(string name, ITraverser<TNode> traverser,
        Action<TraverserConfig<TNode>> config) where TNode : INode
    {
        var childNode = new TraverserStep(node, (ITraverser)traverser, name);
        node.Children.Add(childNode);
        var childConfig = new TraverserConfig<TNode>(childNode);
        config(childConfig);
        return this;
    }

    public GraphPipeline Build()
    {
        return new GraphPipeline(node);
    }
}