using WzJson.V2.Pipeline.Graph;
using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Linear;

public class ConverterConfig<TNode, TResult>(IConverterNode node) where TNode : INode
{
    public ProcessorConfig<TResult, TNextOut> Processor<TNextOut>(IProcessor<TResult, TNextOut> processor)
    {
        var childNode = new ProcessorNode(node, (IProcessor)processor);
        node.AddChild(childNode);
        return new ProcessorConfig<TResult, TNextOut>(childNode);
    }

    public LinearPipeline<TResult> Build()
    {
        IGraphNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline<TResult>(rootNode)
            : throw new InvalidOperationException();
    }
}