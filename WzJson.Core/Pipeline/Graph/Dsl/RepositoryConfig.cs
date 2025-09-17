using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph.Dsl;

public class RepositoryConfig<TNode>(IRepositoryNode node) where TNode : INode
{
    public RepositoryConfig<TNode> Converter<TNextOut>(IConverter<TNode, TNextOut> converter,
        Action<ConverterConfig<TNode, TNextOut>> config)
    {
        var childNode = new ConverterNode(node, (IConverter)converter);
        node.AddChild(childNode);
        var childConfig = new ConverterConfig<TNode, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public RepositoryConfig<TNode> Converter<TNextOut>(Condition when, IConverter<TNode, TNextOut> converter,
        Action<ConverterConfig<TNode, TNextOut>> config)
    {
        if (when.Value) Converter(converter, config);
        return this;
    }
}