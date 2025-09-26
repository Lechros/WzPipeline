using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class ProcessorConfig<TIn, TOut>(IProcessorNode node)
{
    public ProcessorConfig<TOut, TNextOut> Processor<TNextOut>(IProcessor<TOut, TNextOut> processor)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor);
        node.AddChild(childNode);
        return new ProcessorConfig<TOut, TNextOut>(childNode);
    }

    public ExporterConfig<TOut> Exporter(IExporter<TOut> exporter)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter);
        node.AddChild(childNode);
        return new ExporterConfig<TOut>(childNode);
    }

    public LinearPipeline<TOut> Build()
    {
        var holder = new SingleValueHolder<TOut>();
        Exporter(holder);

        IPipelineNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline<TOut>(rootNode, holder)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}