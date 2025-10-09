using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline.Graph;

public class ConverterConfig<TNode, TResult>(IConverterStep step) where TNode : INode
{
    public ConverterConfig<TNode, TResult> Processor<TNextOut>(string name, IProcessor<TResult, TNextOut> processor,
        Action<ProcessorConfig<TResult, TNextOut>> config)
    {
        var childNode = new ProcessorStep(step, (IProcessor)processor, name);
        step.AddChild(childNode);
        var childConfig = new ProcessorConfig<TResult, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public ConverterConfig<TNode, TResult> Exporter(string name, IExporter<TResult> exporter)
    {
        var childNode = new ExporterStep(step, (IExporter)exporter, name);
        step.AddChild(childNode);
        return this;
    }
}