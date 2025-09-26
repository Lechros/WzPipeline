using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ExporterNode(IPipelineNode parent, IExporter exporter) : IExporterNode
{
    public PipelineNodeType Type => PipelineNodeType.Exporter;
    public IPipelineNode? Parent { get; } = parent;
    public IList<IPipelineNode> Children { get; } = [];
    public IExporter Exporter { get; } = exporter;
}