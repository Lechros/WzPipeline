using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}