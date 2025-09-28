using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class ConverterConfig<TNode, TResult>(IConverterStep step) where TNode : INode
{
    public ProcessorConfig<TResult, TNextOut> Processor<TNextOut>(string name, IProcessor<TResult, TNextOut> processor)
    {
        var childNode = new ProcessorStep(step, (IProcessor)processor, name);
        step.AddChild(childNode);
        return new ProcessorConfig<TResult, TNextOut>(childNode);
    }

    public ExporterConfig<TResult> Exporter(string name, IExporter<TResult> exporter)
    {
        var childNode = new ExporterStep(step, (IExporter)exporter, name);
        step.AddChild(childNode);
        return new ExporterConfig<TResult>(childNode);
    }

    public LinearPipeline<TResult> Build()
    {
        var holder = new SingleValueHolder<TResult>();
        Exporter("Result", holder);

        IStep curNode = step;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is PipelineRoot rootNode
            ? new LinearPipeline<TResult>(rootNode, holder)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}