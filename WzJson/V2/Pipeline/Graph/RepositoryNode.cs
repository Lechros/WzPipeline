using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph;

public class RepositoryNode(IGraphNode parent, IRepository repository) : IRepositoryNode
{
    public IGraphNode? Parent { get; } = parent;
    public IList<IGraphNode> Children { get; } = [];
    public IRepository Repository { get; } = repository;

    public void AddChild(IConverterNode node)
    {
        Children.Add(node);
    }
}