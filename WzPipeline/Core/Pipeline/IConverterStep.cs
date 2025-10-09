using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline;

public interface IConverterStep : IStep
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}