namespace WzJson.Core.Pipeline.Abstractions;

public interface IRepository
{
    public IEnumerable<INode> EnumerateNodes();

    public int GetNodeCount();
}

public interface IRepository<out TNode> where TNode : INode
{
    public IEnumerable<TNode> EnumerateNodes();

    public int GetNodeCount();
}