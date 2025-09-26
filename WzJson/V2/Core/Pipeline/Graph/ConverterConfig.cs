using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class ConverterConfig<TNode, TResult>(IConverterNode node) where TNode : INode
{
    public ConverterConfig<TNode, TResult> Processor<TNextOut>(IProcessor<TResult, TNextOut> processor,
        Action<ProcessorConfig<TResult, TNextOut>> config)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor);
        node.AddChild(childNode);
        var childConfig = new ProcessorConfig<TResult, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public ConverterConfig<TNode, TResult> Exporter(IExporter<TResult> exporter)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter);
        node.AddChild(childNode);
        return this;
    }
}