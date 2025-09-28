namespace WzJson.V2.Core.Pipeline;

public interface IStep
{
    public string Name { get; }
    public StepType Type { get; }
    public IStep? Parent { get; }
    public IList<IStep> Children { get; }
}