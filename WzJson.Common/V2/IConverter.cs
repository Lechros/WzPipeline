namespace WzJson.Common.V2;

public interface IConverter<in TNode, out TResult> where TNode : INode
{
    public TResult? Convert(TNode node);
}