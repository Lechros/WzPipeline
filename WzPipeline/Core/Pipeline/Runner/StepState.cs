namespace WzPipeline.Core.Pipeline.Runner;

internal class StepState(string name) : IStepState
{
    private int _count;
    public List<StepState> ChildNodes = [];

    public string Name { get; } = name;
    public IEnumerable<IStepState> Children => ChildNodes;
    public NodeStatus Status { get; private set; }

    public int Count
    {
        get => _count;
        set => _count = value;
    }

    public int? TotalCount { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }

    public TimeSpan Duration
    {
        get
        {
            if (StartTime == null) return TimeSpan.Zero;
            return (EndTime ?? DateTime.Now) - StartTime.Value;
        }
    }

    public void SetTotalCountBeforeStart(int totalCount)
    {
        if (Status != NodeStatus.Pending)
        {
            throw new InvalidOperationException($"The pipeline {Name} should be in Pending state.");
        }

        if (TotalCount != null)
        {
            throw new InvalidOperationException($"The pipeline {Name} TotalCount is already set to {TotalCount}.");
        }

        TotalCount = totalCount;
    }

    public void Start()
    {
        if (TotalCount == null)
        {
            throw new InvalidOperationException($"The pipeline {Name} should have TotalCount set.");
        }

        ExecuteStart();
    }

    public void StartWithTotalCount(int totalCount)
    {
        ExecuteStart();
        TotalCount = totalCount;
    }

    public void IncrementCount()
    {
        Interlocked.Increment(ref _count);
    }

    public void Complete()
    {
        ExecuteComplete();
    }

    public void CompleteWithCount(int count)
    {
        ExecuteComplete();
        Count = count;
    }

    private void ExecuteStart()
    {
        if (Status != NodeStatus.Pending)
        {
            throw new InvalidOperationException($"The pipeline {Name} should be in Pending state.");
        }

        Status = NodeStatus.Running;
        StartTime = DateTime.Now;
    }

    private void ExecuteComplete()
    {
        if (Status != NodeStatus.Running)
        {
            throw new InvalidOperationException($"The pipeline {Name} should be in Running state.");
        }

        Status = NodeStatus.Complete;
        EndTime = DateTime.Now;
    }
}