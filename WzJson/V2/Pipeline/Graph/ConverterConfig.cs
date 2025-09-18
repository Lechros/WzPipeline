using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Graph;

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

    public ConverterConfig<TNode, TResult> Processor<TNextOut>(Condition when, IProcessor<TResult, TNextOut> processor,
        Action<ProcessorConfig<TResult, TNextOut>> config)
    {
        if (when.Value) Processor(processor, config);
        return this;
    }

    public ConverterConfig<TNode, TResult> Exporter(IExporter<TResult> exporter, string path)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter, path);
        node.AddChild(childNode);
        return this;
    }

    public ConverterConfig<TNode, TResult> Exporter(Condition when, IExporter<TResult> exporter, string path)
    {
        if (when.Value) Exporter(exporter, path);
        return this;
    }
}