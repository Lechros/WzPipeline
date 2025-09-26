namespace WzJson.V2.Core.Pipeline.Runner;

public class NodeProgress
{
    public int TotalCount { get; init; }
    public int CompletedCount { get; init; }
    public NodeStatus Status { get; init; }
}

public enum NodeStatus
{
    Pending,
    Running,
    Complete,
}