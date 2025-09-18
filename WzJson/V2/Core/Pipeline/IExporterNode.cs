using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}