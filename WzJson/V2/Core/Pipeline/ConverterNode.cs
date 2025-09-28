using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ConverterNode(IPipelineNode parent, IConverter converter, string name) : IConverterNode
{
    public string Name => name;
    public PipelineNodeType Type => PipelineNodeType.Converter;
    public IPipelineNode? Parent { get; } = parent;
    public IList<IPipelineNode> Children { get; } = [];
    public IConverter Converter { get; } = converter;

    public void AddChild(IProcessorNode node)
    {
        Children.Add(node);
    }

    public void AddChild(IExporterNode node)
    {
        Children.Add(node);
    }
}