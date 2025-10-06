using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public class TraverserStep(IStep parent, ITraverser traverser, string name) : ITraverserStep
{
    public string Name => name;
    public StepType Type => StepType.Traverser;
    public IStep? Parent { get; } = parent;
    public IList<IStep> Children { get; } = [];
    public ITraverser Traverser { get; } = traverser;

    public void AddChild(IConverterStep step)
    {
        Children.Add(step);
    }
}