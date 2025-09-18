using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Graph;

public class GraphPipelineConfig(RootNode node)
{
    public GraphPipelineConfig Repository<TNode>(IRepository<TNode> repository,
        Action<RepositoryConfig<TNode>> config) where TNode : INode
    {
        var childNode = new RepositoryNode(node, (IRepository)repository);
        node.Children.Add(childNode);
        var childConfig = new RepositoryConfig<TNode>(childNode);
        config(childConfig);
        return this;
    }

    public GraphPipelineConfig Repository<TNode>(Condition when, IRepository<TNode> repository,
        Action<RepositoryConfig<TNode>> config) where TNode : INode
    {
        if (when.Value) Repository(repository, config);
        return this;
    }

    public GraphPipeline Build()
    {
        return new GraphPipeline(node);
    }
}