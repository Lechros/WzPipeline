namespace WzJson.V2.Pipeline.Graph;

public class GraphPipeline(RootNode root)
{
    public void Run()
    {
        GraphPipelineRunner.Run(root);
    }

    public void Prune()
    {
        PruneNode(root);
    }

    private void PruneNode(IGraphNode node)
    {
        var children = node.Children;
        for (int i = children.Count - 1; i >= 0; i--)
        {
            var child = children[i];

            if (child.Children.Count > 0)
            {
                PruneNode(child);
            }

            if (child is not IExporterNode && child.Children.Count == 0)
            {
                children.RemoveAt(i);
            }
        }
    }
}