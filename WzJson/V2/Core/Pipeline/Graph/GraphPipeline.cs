using WzJson.V2.Core.Pipeline.Runner;

namespace WzJson.V2.Core.Pipeline.Graph;

public class GraphPipeline(RootNode root)
{
    public GraphPipelineResult Run(IProgress<INodeState> progress)
    {
        var ctx = PipelineRunner.Run(root, progress);
        return new GraphPipelineResult(ctx.GetRootState());
    }
}

public class GraphPipelineResult
{
    public INodeState State;

    internal GraphPipelineResult(INodeState state)
    {
        State = state;
    }
}