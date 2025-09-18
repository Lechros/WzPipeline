using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface ITraverserNode : IGraphNode
{
    public ITraverser Traverser { get; }

    public void AddChild(IConverterNode node);
}