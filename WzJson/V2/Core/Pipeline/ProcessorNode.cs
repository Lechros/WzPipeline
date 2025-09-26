using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ProcessorNode(IPipelineNode parent, IProcessor processor) : IProcessorNode
{
    public PipelineNodeType Type => PipelineNodeType.Processor;
    public IPipelineNode? Parent { get; } = parent;
    public IList<IPipelineNode> Children { get; } = [];
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