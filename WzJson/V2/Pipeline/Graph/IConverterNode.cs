using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph;

public interface IConverterNode : IGraphNode
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}