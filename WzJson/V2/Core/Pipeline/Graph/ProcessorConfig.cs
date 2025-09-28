using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Graph;

public class ProcessorConfig<TIn, TOut>(IProcessorStep step)
{
    public ProcessorConfig<TIn, TOut> Processor<TNextOut>(string name, IProcessor<TOut, TNextOut> processor,
        Action<ProcessorConfig<TOut, TNextOut>> config)
    {
        var childNode = new ProcessorStep(step, (IProcessor)processor, name);
        step.AddChild(childNode);
        var childConfig = new ProcessorConfig<TOut, TNextOut>(childNode);
        config(childConfig);
        return this;
    }

    public ProcessorConfig<TIn, TOut> Exporter(string name, IExporter<TOut> exporter)
    {
        var childNode = new ExporterStep(step, (IExporter)exporter, name);
        step.AddChild(childNode);
        return this;
    }
}