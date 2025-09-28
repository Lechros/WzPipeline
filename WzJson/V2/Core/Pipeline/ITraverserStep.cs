using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface ITraverserStep : IStep
{
    public ITraverser Traverser { get; }

    public void AddChild(IConverterStep step);
}