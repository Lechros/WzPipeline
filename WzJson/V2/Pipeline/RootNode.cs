using WzJson.V2.Pipeline.Graph;

namespace WzJson.V2.Pipeline;

public class RootNode : IGraphNode
{
    public IGraphNode? Parent => null;
    public IList<IGraphNode> Children { get; } = [];
}