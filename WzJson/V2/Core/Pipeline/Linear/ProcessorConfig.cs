using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class ProcessorConfig<TIn, TOut>(IProcessorNode node)
{
    public ProcessorConfig<TOut, TNextOut> Processor<TNextOut>(string name, IProcessor<TOut, TNextOut> processor)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor, name);
        node.AddChild(childNode);
        return new ProcessorConfig<TOut, TNextOut>(childNode);
    }

    public ExporterConfig<TOut> Exporter(string name, IExporter<TOut> exporter)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter, name);
        node.AddChild(childNode);
        return new ExporterConfig<TOut>(childNode);
    }

    public LinearPipeline<TOut> Build()
    {
        var holder = new SingleValueHolder<TOut>();
        Exporter("Result", holder);

        IPipelineNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline<TOut>(rootNode, holder)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}