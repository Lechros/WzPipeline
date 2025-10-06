using WzJson.Core.Pipeline.Runner;

namespace WzJson.Core.Pipeline.Linear;

public class LinearPipeline<T>(PipelineRoot root, SingleValueHolder<T> holder)
{
    public LinearPipelineResult<T> Run(IProgress<IStepState> progress)
    {
        holder.ClearValue();
        var ctx = PipelineRunner.Run(root, progress);
        return new LinearPipelineResult<T>(ctx.GetRootState(), holder.Value);
    }
}

public class LinearPipeline(PipelineRoot root)
{
    public LinearPipelineResult Run(IProgress<IStepState> progress)
    {
        var ctx = PipelineRunner.Run(root, progress);
        return new LinearPipelineResult(ctx.GetRootState());
    }
}

public class LinearPipelineResult<T>
{
    public IStepState State;
    public T Value;

    internal LinearPipelineResult(IStepState state, T value)
    {
        State = state;
        Value = value;
    }
}

public class LinearPipelineResult
{
    public IStepState State;

    internal LinearPipelineResult(IStepState state)
    {
        State = state;
    }
}