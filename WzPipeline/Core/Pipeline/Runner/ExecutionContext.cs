namespace WzPipeline.Core.Pipeline.Runner;

internal class ExecutionContext
{
    private readonly StepState _rootState;
    private readonly Dictionary<IStep, StepState> _states = new();
    private readonly IProgress<IStepState>? _progress;

    private readonly List<ITraverserStep> _traverserSteps = [];

    internal ExecutionContext(PipelineRoot root, IProgress<IStepState>? progress = null)
    {
        _rootState = InitializeDfs(root);
        _progress = progress;
    }

    public IReadOnlyList<ITraverserStep> TraverserSteps => _traverserSteps;

    public StepState GetStepState(IStep step)
    {
        return _states[step];
    }

    public ExecutionContext SetTotalCountBeforeStart(int totalCount, params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.SetTotalCountBeforeStart(totalCount);
        }

        return this;
    }

    public ExecutionContext Start(params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.Start();
        }

        return this;
    }

    public ExecutionContext StartWithTotalCount(int totalCount, params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.StartWithTotalCount(totalCount);
        }

        return this;
    }

    public ExecutionContext IncrementCount(params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.IncrementCount();
        }

        return this;
    }

    public ExecutionContext Complete(params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.Complete();
        }

        return this;
    }

    public ExecutionContext CompleteWithCount(int count, params IStep[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetStepState(node);
            state.CompleteWithCount(count);
        }

        return this;
    }

    public IStepState GetRootState()
    {
        return _rootState;
    }

    public void Report()
    {
        _progress?.Report(_rootState);
    }

    private StepState InitializeDfs(IStep node)
    {
        var nodeInfo = new StepState(node.Name);

        _states[node] = nodeInfo;
        if (node.Type == StepType.Traverser)
        {
            _traverserSteps.Add((ITraverserStep)node);
        }

        foreach (var child in node.Children)
        {
            nodeInfo.ChildNodes.Add(InitializeDfs(child));
        }

        return nodeInfo;
    }
}