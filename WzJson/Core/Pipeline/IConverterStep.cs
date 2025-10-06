using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public interface IConverterStep : IStep
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}