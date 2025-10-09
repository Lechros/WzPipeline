using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline.Linear;

public class TraverserConfig<TNode>(ITraverserStep step) where TNode : INode
{
    public ConverterConfig<TNode, TNextOut> Converter<TNextOut>(string name, IConverter<TNode, TNextOut> converter)
    {
        var childNode = new ConverterStep(step, (IConverter)converter, name);
        step.AddChild(childNode);
        return new ConverterConfig<TNode, TNextOut>(childNode);
    }
}