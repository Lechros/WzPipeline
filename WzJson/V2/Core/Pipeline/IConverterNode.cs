using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IConverterNode : IPipelineNode
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}