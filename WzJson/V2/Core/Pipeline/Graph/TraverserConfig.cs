using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class TraverserConfig<TNode>(ITraverserNode node) where TNode : INode
{
    public TraverserConfig<TNode> Converter<TNextOut>(IConverter<TNode, TNextOut> converter,
        Action<ConverterConfig<TNode, TNextOut>> config)
    {
        var childNode = new ConverterNode(node, (IConverter)converter);
        node.AddChild(childNode);
        var childConfig = new ConverterConfig<TNode, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public TraverserConfig<TNode> Converter<TNextOut>(Condition when, IConverter<TNode, TNextOut> converter,
        Action<ConverterConfig<TNode, TNextOut>> config)
    {
        if (when.Value) Converter(converter, config);
        return this;
    }
}