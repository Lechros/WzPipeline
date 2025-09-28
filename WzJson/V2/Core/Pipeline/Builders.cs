using WzJson.V2.Core.Pipeline.Graph;
using WzJson.V2.Core.Pipeline.Linear;

namespace WzJson.V2.Core.Pipeline;

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