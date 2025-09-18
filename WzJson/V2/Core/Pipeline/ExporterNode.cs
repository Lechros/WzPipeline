using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ExporterNode(IGraphNode parent, IExporter exporter, string path) : IExporterNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
    public IExporter Exporter { get; } = exporter;
    public string Path { get; } = path;
}