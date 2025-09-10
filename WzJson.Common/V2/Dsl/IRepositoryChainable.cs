namespace WzJson.Common.V2.Dsl;

public interface IRepositoryChainable<out TThis>
{
    public IEnumerable<IRepositoryBuilder> ChainedChainedRepositories { get; }

    public TThis Repository<TNode>(
        INodeRepository2<TNode> next,
        Action<RepositoryBuilder<TNode>> config) where TNode : INode;

    public TThis Repository<TNode>(Condition when,
        INodeRepository2<TNode> next,
        Action<RepositoryBuilder<TNode>> config) where TNode : INode;
}