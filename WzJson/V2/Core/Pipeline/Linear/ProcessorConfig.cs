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

    public LinearPipeline<TOut> Build()
    {
        IGraphNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline<TOut>(rootNode)
            : throw new InvalidOperationException();
    }
}