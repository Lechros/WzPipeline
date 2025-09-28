using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class TraverserConfig<TNode>(ITraverserNode node) where TNode : INode
{
    public ConverterConfig<TNode, TNextOut> Converter<TNextOut>(string name, IConverter<TNode, TNextOut> converter)
    {
        var childNode = new ConverterNode(node, (IConverter)converter, name);
        node.AddChild(childNode);
        return new ConverterConfig<TNode, TNextOut>(childNode);
    }
}