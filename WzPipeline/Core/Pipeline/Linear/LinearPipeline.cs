using WzPipeline.Core.Pipeline.Runner;

namespace WzPipeline.Core.Pipeline.Linear;

public class LinearPipeline<T>(PipelineRoot root, SingleValueHolder<T> holder)
{
    public LinearPipelineResult<T> Run(IPipelineRunner runner, IProgress<IStepState>? progress = null)
    {
        holder.ClearValue();
        var state = runner.Run(root, progress);
        return new LinearPipelineResult<T>(state, holder.Value);
    }
}

public class LinearPipeline(PipelineRoot root)
{
    public LinearPipelineResult Run(IPipelineRunner runner, IProgress<IStepState>? progress = null)
    {
        var state = runner.Run(root, progress);
        return new LinearPipelineResult(state);
    }
}

public class LinearPipelineResult<T>
{
    public IStepState State { get; }
    public T Value { get; }

    internal LinearPipelineResult(IStepState state, T value)
    {
        State = state;
        Value = value;
    }
}

public class LinearPipelineResult
{
    public IStepState State { get; }

    internal LinearPipelineResult(IStepState state)
    {
        State = state;
    }
}