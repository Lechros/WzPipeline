using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph.Dsl;

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

    public ProcessorConfig<TIn, TOut> Exporter(IExporter<TOut> exporter, string path)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter, path);
        node.AddChild(childNode);
        return this;
    }

    public ProcessorConfig<TIn, TOut> Exporter(Condition when, IExporter<TOut> exporter, string path)
    {
        if (when.Value) Exporter(exporter, path);
        return this;
    }
}