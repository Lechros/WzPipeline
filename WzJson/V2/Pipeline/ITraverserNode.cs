using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline;

public interface ITraverserNode : IGraphNode
{
    public ITraverser Traverser { get; }

    public void AddChild(IConverterNode node);
}