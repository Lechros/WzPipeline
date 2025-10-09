using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline;

public interface IExporterStep : IStep
{
    public IExporter Exporter { get; }
}