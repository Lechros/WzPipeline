using WzJson.V2.Pipeline.Graph;
using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Linear;

public class TraverserConfig<TNode>(ITraverserNode node) where TNode : INode
{
    public ConverterConfig<TNode, TNextOut> Converter<TNextOut>(IConverter<TNode, TNextOut> converter)
    {
        var childNode = new ConverterNode(node, (IConverter)converter);
        node.AddChild(childNode);
        return new ConverterConfig<TNode, TNextOut>(childNode);
    }
}