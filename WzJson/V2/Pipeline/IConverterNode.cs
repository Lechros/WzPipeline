using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline;

public interface IConverterNode : IGraphNode
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}