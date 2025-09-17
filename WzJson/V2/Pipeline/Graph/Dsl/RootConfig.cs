using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph.Dsl;

public class RootConfig(GraphSystem.RootNode node)
{
    public RootConfig Repository<TNode>(IRepository<TNode> repository,
        Action<RepositoryConfig<TNode>> config) where TNode : INode
    {
        var childNode = new RepositoryNode(node, (IRepository)repository);
        node.Children.Add(childNode);
        var childConfig = new RepositoryConfig<TNode>(childNode);
        config(childConfig);
        return this;
    }

    public RootConfig Repository<TNode>(Condition when, IRepository<TNode> repository,
        Action<RepositoryConfig<TNode>> config) where TNode : INode
    {
        if (when.Value) Repository(repository, config);
        return this;
    }

    public GraphSystem Build()
    {
        return new GraphSystem(node);
    }
}