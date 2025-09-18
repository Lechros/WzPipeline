using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class TraverserConfig<TNode>(ITraverserNode node) where TNode : INode
{
    public ConverterConfig<TNode, TNextOut> Converter<TNextOut>(IConverter<TNode, TNextOut> converter)
    {
        var childNode = new ConverterNode(node, (IConverter)converter);
        node.AddChild(childNode);
        return new ConverterConfig<TNode, TNextOut>(childNode);
    }
}