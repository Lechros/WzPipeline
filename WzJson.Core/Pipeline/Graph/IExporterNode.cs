using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}