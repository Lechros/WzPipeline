namespace WzJson.V2.Core.Pipeline;

public class PipelineRoot(string name) : IStep
{
    public string Name => name;
    public StepType Type => StepType.Default;
    public IStep? Parent => null;
    public IList<IStep> Children { get; } = [];
}