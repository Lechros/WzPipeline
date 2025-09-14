using WzJson.Core.Abstractions;

namespace WzJson.Core.Pipeline;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}