using WzPipeline.Core.Stereotype;

namespace WzPipeline.Core.Pipeline;

public interface IProcessorStep : IStep
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}