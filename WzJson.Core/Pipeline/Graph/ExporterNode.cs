using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph;

public class ExporterNode(IGraphNode parent, IExporter exporter, string path) : IExporterNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
    public IExporter Exporter { get; } = exporter;
    public string Path { get; } = path;
}