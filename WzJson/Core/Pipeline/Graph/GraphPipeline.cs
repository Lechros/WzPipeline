using WzJson.Core.Pipeline.Runner;

namespace WzJson.Core.Pipeline.Graph;

public class GraphPipeline(PipelineRoot root)
{
    public GraphPipelineResult Run(IPipelineRunner runner, IProgress<IStepState>? progress = null)
    {
        var state = runner.Run(root, progress);
        return new GraphPipelineResult(state);
    }
}

public class GraphPipelineResult
{
    public IStepState State { get; }

    internal GraphPipelineResult(IStepState state)
    {
        State = state;
    }
}