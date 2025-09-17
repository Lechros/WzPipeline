using WzJson.V2.Pipeline.Graph.Dsl;

namespace WzJson.V2.Pipeline.Graph;

public class GraphSystem(GraphSystem.RootNode root)
{
    public static RootConfig Builder()
    {
        RootNode root = new RootNode();
        return new RootConfig(root);
    }

    public void Run()
    {
        GraphSystemRunner.Run(root);
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

    public class RootNode : IGraphNode
    {
        public IGraphNode? Parent => null;
        public IList<IGraphNode> Children { get; } = [];
    }
}