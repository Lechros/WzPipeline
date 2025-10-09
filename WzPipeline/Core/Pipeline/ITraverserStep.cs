using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline;

public interface ITraverserStep : IStep
{
    public ITraverser Traverser { get; }

    public void AddChild(IConverterStep step);
}