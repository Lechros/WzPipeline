namespace WzJson.V2.Core.Pipeline.Runner;

internal class ExecutionContext
{
    private readonly NodeState _rootState;
    private readonly Dictionary<IPipelineNode, NodeState> _states = new();
    private readonly IProgress<INodeState>? _progress;

    private readonly List<ITraverserNode> _traverserNodes = [];

    internal ExecutionContext(RootNode root, IProgress<INodeState>? progress = null)
    {
        Root = root;
        _rootState = InitializeDfs(root);
        _progress = progress;
    }

    public RootNode Root { get; }
    public IReadOnlyList<ITraverserNode> TraverserNodes => _traverserNodes;

    public NodeState GetNodeState(IPipelineNode pipelineNode)
    {
        return _states[pipelineNode];
    }

    public ExecutionContext SetTotalCountBeforeStart(int totalCount, params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.SetTotalCountBeforeStart(totalCount);
        }

        return this;
    }

    public ExecutionContext Start(params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.Start();
        }

        return this;
    }

    public ExecutionContext StartWithTotalCount(int totalCount, params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.StartWithTotalCount(totalCount);
        }

        return this;
    }

    public ExecutionContext IncrementCount(params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.IncrementCount();
        }

        return this;
    }

    public ExecutionContext Complete(params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.Complete();
        }

        return this;
    }

    public ExecutionContext CompleteWithCount(int count, params IPipelineNode[] nodes)
    {
        foreach (var node in nodes)
        {
            var state = GetNodeState(node);
            state.CompleteWithCount(count);
        }

        return this;
    }

    public INodeState GetRootState()
    {
        return _rootState;
    }

    public void Report()
    {
        _progress?.Report(_rootState);
    }

    private NodeState InitializeDfs(IPipelineNode node)
    {
        var nodeInfo = new NodeState(node.Name);

        _states[node] = nodeInfo;
        if (node.Type == PipelineNodeType.Traverser)
        {
            _traverserNodes.Add((ITraverserNode)node);
        }

        foreach (var child in node.Children)
        {
            nodeInfo.ChildNodes.Add(InitializeDfs(child));
        }

        return nodeInfo;
    }
}