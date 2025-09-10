namespace WzJson.Common.V2.Dsl;

public interface IConverterBuilder<in TNode> where TNode : INode
{
    public IConverter<TNode, object> Get();
}