namespace WzJson.V2.Core.Pipeline.Runner;

public interface INodeState
{
    public string Name { get; }
    public IEnumerable<INodeState> Children { get; }

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