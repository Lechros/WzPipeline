using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class ProcessorConfig<TIn, TOut>(IProcessorNode node)
{
    public ProcessorConfig<TIn, TOut> Processor<TNextOut>(IProcessor<TOut, TNextOut> processor,
        Action<ProcessorConfig<TOut, TNextOut>> config)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor);
        node.AddChild(childNode);
        var childConfig = new ProcessorConfig<TOut, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public ProcessorConfig<TIn, TOut> Processor<TNextOut>(Condition when, IProcessor<TOut, TNextOut> processor,
        Action<ProcessorConfig<TOut, TNextOut>> config)
    {
        if (when.Value) Processor(processor, config);
        return this;
    }

    public ProcessorConfig<TIn, TOut> Exporter(IExporter<TOut> exporter)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter);
        node.AddChild(childNode);
        return this;
    }

    public ProcessorConfig<TIn, TOut> Exporter(Condition when, IExporter<TOut> exporter)
    {
        if (when.Value) Exporter(exporter);
        return this;
    }
}