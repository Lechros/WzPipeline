using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ExporterStep(IStep parent, IExporter exporter, string name) : IExporterStep
{
    public string Name => name;
    public StepType Type => StepType.Exporter;
    public IStep? Parent { get; } = parent;
    public IList<IStep> Children { get; } = [];
    public IExporter Exporter { get; } = exporter;
}