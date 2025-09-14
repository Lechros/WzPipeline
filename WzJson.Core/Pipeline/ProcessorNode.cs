using WzJson.Core.Abstractions;

namespace WzJson.Core.Pipeline;

public class ProcessorNode(IGraphNode parent, IProcessor processor) : IProcessorNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
    public IProcessor Processor { get; } = processor;

    public void AddChild(IProcessorNode node)
    {
        Children.Add(node);
    }

    public void AddChild(IExporterNode node)
    {
        Children.Add(node);
    }
}