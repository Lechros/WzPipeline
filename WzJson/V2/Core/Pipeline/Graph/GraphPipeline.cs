using WzJson.V2.Core.Pipeline.Runner;

namespace WzJson.V2.Core.Pipeline.Graph;

public class GraphPipeline(RootNode root)
{
    public void Run()
    {
        PipelineRunner.Run(root);
    }
}