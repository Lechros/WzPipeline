namespace WzJson.Common.V2;

public interface INodeRepository2<out TNode> where TNode : INode
{
    public IEnumerable<TNode> EnumerateNodes();

    public int GetNodeCount();
}