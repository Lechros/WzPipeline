using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public class ConverterStep(IStep parent, IConverter converter, string name) : IConverterStep
{
    public string Name => name;
    public StepType Type => StepType.Converter;
    public IStep? Parent { get; } = parent;
    public IList<IStep> Children { get; } = [];
    public IConverter Converter { get; } = converter;

    public void AddChild(IProcessorStep step)
    {
        Children.Add(step);
    }

    public void AddChild(IExporterStep step)
    {
        Children.Add(step);
    }
}