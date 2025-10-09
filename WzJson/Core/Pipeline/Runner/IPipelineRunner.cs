namespace WzJson.Core.Pipeline.Runner;

public interface IPipelineRunner
{
    public IStepState Run(PipelineRoot root, IProgress<IStepState>? progress = null);
}