using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}