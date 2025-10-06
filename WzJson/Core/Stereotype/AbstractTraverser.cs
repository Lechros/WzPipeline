namespace WzJson.Core.Stereotype;

public abstract class AbstractTraverser<TNode> : ITraverser<TNode>, ITraverser where TNode : INode
{
    public abstract IEnumerable<TNode> EnumerateNodes();

    public abstract int GetNodeCount();

    IEnumerable<INode> ITraverser.EnumerateNodes() => EnumerateNodes().Cast<INode>();

    int ITraverser.GetNodeCount() => GetNodeCount();
}