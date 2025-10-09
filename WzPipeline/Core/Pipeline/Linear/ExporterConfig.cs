namespace WzPipeline.Core.Pipeline.Linear;

public class ExporterConfig<TResult>(IExporterStep step)
{
    public LinearPipeline Build()
    {
        IStep curNode = step;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is PipelineRoot rootNode
            ? new LinearPipeline(rootNode)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}