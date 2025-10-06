using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline.Linear;

public class ProcessorConfig<TIn, TOut>(IProcessorStep step)
{
    public ProcessorConfig<TOut, TNextOut> Processor<TNextOut>(string name, IProcessor<TOut, TNextOut> processor)
    {
        var childNode = new ProcessorStep(step, (IProcessor)processor, name);
        step.AddChild(childNode);
        return new ProcessorConfig<TOut, TNextOut>(childNode);
    }

    public ExporterConfig<TOut> Exporter(string name, IExporter<TOut> exporter)
    {
        var childNode = new ExporterStep(step, (IExporter)exporter, name);
        step.AddChild(childNode);
        return new ExporterConfig<TOut>(childNode);
    }

    public LinearPipeline<TOut> Build()
    {
        var holder = new SingleValueHolder<TOut>();
        Exporter("Result", holder);

        IStep curNode = step;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is PipelineRoot rootNode
            ? new LinearPipeline<TOut>(rootNode, holder)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}