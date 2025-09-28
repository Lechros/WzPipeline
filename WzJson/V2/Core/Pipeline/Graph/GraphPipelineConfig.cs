using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class GraphPipelineConfig(RootNode node)
{
    public GraphPipelineConfig Traverser<TNode>(string name, ITraverser<TNode> traverser,
        Action<TraverserConfig<TNode>> config) where TNode : INode
    {
        var childNode = new TraverserNode(node, (ITraverser)traverser, name);
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