using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public interface ITraverserStep : IStep
{
    public ITraverser Traverser { get; }

    public void AddChild(IConverterStep step);
}