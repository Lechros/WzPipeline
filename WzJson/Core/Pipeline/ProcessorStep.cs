using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public class ProcessorStep(IStep parent, IProcessor processor, string name) : IProcessorStep
{
    public string Name => name;
    public StepType Type => StepType.Processor;
    public IStep? Parent { get; } = parent;
    public IList<IStep> Children { get; } = [];
    public IProcessor Processor { get; } = processor;

    public void AddChild(IProcessorStep step)
    {
        Children.Add(step);
    }

    public void AddChild(IExporterStep step)
    {
        Children.Add(step);
    }
}