using WzJson.V2.Pipeline.Graph;
using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Linear;

public class LinearPipelineConfig(RootNode node)
{
    public RepositoryConfig<TNode> Repository<TNode>(IRepository<TNode> repository) where TNode : INode
    {
        var childNode = new RepositoryNode(node, (IRepository)repository);
        node.Children.Add(childNode);
        return new RepositoryConfig<TNode>(childNode);
    }
}