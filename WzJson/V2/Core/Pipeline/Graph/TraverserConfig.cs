using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class TraverserConfig<TNode>(ITraverserStep step) where TNode : INode
{
    public TraverserConfig<TNode> Converter<TNextOut>(string name, IConverter<TNode, TNextOut> converter,
        Action<ConverterConfig<TNode, TNextOut>> config)
    {
        var childNode = new ConverterStep(step, (IConverter)converter, name);
        step.AddChild(childNode);
        var childConfig = new ConverterConfig<TNode, TNextOut>(childNode);
        config(childConfig);
        return this;
    }
}