using WzJson.Core.Abstractions;

namespace WzJson.Core.Pipeline;

public interface IConverterNode : IGraphNode
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}