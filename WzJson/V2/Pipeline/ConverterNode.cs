using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Graph;

public class ConverterNode(IGraphNode parent, IConverter converter) : IConverterNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
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