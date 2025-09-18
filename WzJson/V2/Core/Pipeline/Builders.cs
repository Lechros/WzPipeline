using WzJson.V2.Core.Pipeline.Graph;
using WzJson.V2.Core.Pipeline.Linear;

namespace WzJson.V2.Core.Pipeline;

public static class Builders
{
    public static LinearPipelineConfig LinearPipelineBuilder()
    {
        var root = new RootNode();
        return new LinearPipelineConfig(root);
    }

    public static GraphPipelineConfig GraphPipelineBuilder()
    {
        var root = new RootNode();
        return new GraphPipelineConfig(root);
    }
}