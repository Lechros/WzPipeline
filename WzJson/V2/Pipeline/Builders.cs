using WzJson.V2.Pipeline.Graph;
using WzJson.V2.Pipeline.Linear;

namespace WzJson.V2.Pipeline;

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