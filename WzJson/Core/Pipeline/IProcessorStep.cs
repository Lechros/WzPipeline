using WzJson.Core.Stereotype;

namespace WzJson.Core.Pipeline;

public interface IProcessorStep : IStep
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}