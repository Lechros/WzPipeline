using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IExporterNode : IPipelineNode
{
    public IExporter Exporter { get; }
}