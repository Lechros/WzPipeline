namespace WzJson.V2.Stereotype;

public abstract class AbstractRepository<TNode> : IRepository<TNode>, IRepository where TNode : INode
{
    public abstract IEnumerable<TNode> EnumerateNodes();

    public abstract int GetNodeCount();

    IEnumerable<INode> IRepository.EnumerateNodes() => EnumerateNodes().Cast<INode>();

    int IRepository.GetNodeCount() => GetNodeCount();
}