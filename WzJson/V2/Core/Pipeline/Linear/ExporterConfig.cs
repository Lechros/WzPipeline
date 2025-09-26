namespace WzJson.V2.Core.Pipeline.Linear;

public class ExporterConfig<TResult>(IExporterNode node)
{
    public LinearPipeline Build()
    {
        IPipelineNode curNode = node;
        while (curNode.Parent != null)
            curNode = curNode.Parent;

        return curNode is RootNode rootNode
            ? new LinearPipeline(rootNode)
            : throw new InvalidOperationException("Invalid Linear Pipeline");
    }
}