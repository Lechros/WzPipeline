namespace WzJson.Core.Stereotype;

public interface ITraverser
{
    public IEnumerable<INode> EnumerateNodes();

    public int GetNodeCount();
}

public interface ITraverser<out TNode> where TNode : INode
{
    public IEnumerable<TNode> EnumerateNodes();

    public int GetNodeCount();
}