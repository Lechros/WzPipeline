using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IProcessorStep : IStep
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorStep step);
    public void AddChild(IExporterStep step);
}