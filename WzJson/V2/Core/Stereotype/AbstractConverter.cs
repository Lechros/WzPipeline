namespace WzJson.V2.Core.Stereotype;

public abstract class AbstractConverter<TNode, TResult> : IConverter<TNode, TResult>, IConverter where TNode : INode
{
    public abstract TResult? Convert(TNode node);

    public object? Convert(INode value)
    {
        return Convert((TNode)value);
    }
}