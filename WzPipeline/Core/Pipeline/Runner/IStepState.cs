namespace WzPipeline.Core.Pipeline.Runner;

public interface IStepState
{
    public string Name { get; }
    public IEnumerable<IStepState> Children { get; }

    public NodeStatus Status { get; }
    public int Count { get; }
    public int? TotalCount { get; }
    public DateTime? StartTime { get; }
    public DateTime? EndTime { get; }
    public TimeSpan Duration { get; }
}

public enum NodeStatus
{
    Pending,
    Running,
    Complete,
}