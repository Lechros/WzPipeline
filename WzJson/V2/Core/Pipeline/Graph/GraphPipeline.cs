using WzJson.V2.Core.Pipeline.Runner;

namespace WzJson.V2.Core.Pipeline.Graph;

public class GraphPipeline(PipelineRoot root)
{
    public GraphPipelineResult Run(IProgress<IStepState> progress)
    {
        var ctx = PipelineRunner.Run(root, progress);
        return new GraphPipelineResult(ctx.GetRootState());
    }
}

public class GraphPipelineResult
{
    public IStepState State;

    internal GraphPipelineResult(IStepState state)
    {
        State = state;
    }
}