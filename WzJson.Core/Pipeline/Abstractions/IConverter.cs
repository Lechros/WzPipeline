namespace WzJson.Core.Pipeline.Abstractions;

public interface IConverter
{
    public object? Convert(INode value);
}

public interface IConverter<in TNode, out TResult> where TNode : INode
{
    public TResult? Convert(TNode node);
}