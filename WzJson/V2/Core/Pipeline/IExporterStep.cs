using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IExporterStep : IStep
{
    public IExporter Exporter { get; }
}