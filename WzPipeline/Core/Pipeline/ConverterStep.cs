using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline;

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