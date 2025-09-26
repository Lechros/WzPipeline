using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class ConverterConfig<TNode, TResult>(IConverterNode node) where TNode : INode
{
    public ProcessorConfig<TResult, TNextOut> Processor<TNextOut>(IProcessor<TResult, TNextOut> processor)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor);
        node.AddChild(childNode);
        return new ProcessorConfig<TResult, TNextOut>(childNode);
    }

    public ExporterConfig<TResult> Exporter(IExporter<TResult> exporter)
    {
        var childNode = new ExporterNode(node, (IExporter)exporter);
        node.AddChild(childNode);
        return new ExporterConfig<TResult>(childNode);
    }

    public LinearPipeline<TResult> Build()
    {
        var holder = new SingleValueHolder<TResult>();
        Exporter(holder);

        IPipelineNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline<TResult>(rootNode, holder)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}