namespace WzJson.Common.V2.Dsl;

public class SystemBuilder : IRepositoryChainable<SystemBuilder>
{
    private readonly List<IRepositoryBuilder> _chainedRepositories;

    private SystemBuilder()
    {
        _chainedRepositories = [];
    }

    public IEnumerable<IRepositoryBuilder> ChainedChainedRepositories => _chainedRepositories;

    public SystemBuilder Repository<TNode>(INodeRepository2<TNode> next,
        Action<RepositoryBuilder<TNode>> config)
        where TNode : INode
    {
        var repositoryBuilder = new RepositoryBuilder<TNode>(next);
        config(repositoryBuilder);
        _chainedRepositories.Add(repositoryBuilder);
        return this;
    }

    public SystemBuilder Repository<TNode>(Condition when, INodeRepository2<TNode> next,
        Action<RepositoryBuilder<TNode>> config) where TNode : INode
    {
        if (when.Value) Repository(next, config);
        return this;
    }

    public static SystemBuilder Create()
    {
        return new SystemBuilder();
    }
}