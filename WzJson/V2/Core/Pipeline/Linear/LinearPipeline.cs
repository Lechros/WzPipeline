using WzJson.V2.Core.Pipeline.Runner;

namespace WzJson.V2.Core.Pipeline.Linear;

public class LinearPipeline<T>(RootNode root, SingleValueHolder<T> holder)
{
    public LinearPipelineResult<T> Run(IProgress<INodeState> progress)
    {
        holder.ClearValue();
        var ctx = PipelineRunner.Run(root, progress);
        return new LinearPipelineResult<T>(ctx.GetRootState(), holder.Value);
    }
}

public class LinearPipeline(RootNode root)
{
    public LinearPipelineResult Run(IProgress<INodeState> progress)
    {
        var ctx = PipelineRunner.Run(root, progress);
        return new LinearPipelineResult(ctx.GetRootState());
    }
}

public class LinearPipelineResult<T>
{
    public INodeState State;
    public T Value;

    internal LinearPipelineResult(INodeState state, T value)
    {
        State = state;
        Value = value;
    }
}

public class LinearPipelineResult
{
    public INodeState State;

    internal LinearPipelineResult(INodeState state)
    {
        State = state;
    }
}