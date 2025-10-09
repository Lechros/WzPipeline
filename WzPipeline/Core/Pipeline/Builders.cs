using WzPipeline.Core.Pipeline.Graph;
using WzPipeline.Core.Pipeline.Linear;

namespace WzPipeline.Core.Pipeline;

public static class Builders
{
    public static LinearPipelineConfig LinearPipelineBuilder(string pipelineName)
    {
        var root = new PipelineRoot(pipelineName);
        return new LinearPipelineConfig(root);
    }

    public static GraphPipelineConfig GraphPipelineBuilder(string pipelineName)
    {
        var root = new PipelineRoot(pipelineName);
        return new GraphPipelineConfig(root);
    }
}