using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public interface IExporterStep : IStep
{
    public IExporter Exporter { get; }
}