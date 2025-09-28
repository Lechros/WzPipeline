using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IConverterStep : IStep
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}