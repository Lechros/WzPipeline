using WzJson.V2.Core.Pipeline.Runner;

namespace WzJson.V2.Core.Pipeline.Linear;

public class LinearPipeline<T>(RootNode root, SingleValueHolder<T> holder)
{
    public T Run()
    {
        holder.ClearValue();
        PipelineRunner.Run(root);
        return holder.Value;
    }
}

public class LinearPipeline(RootNode root)
{
    public void Run()
    {
        PipelineRunner.Run(root);
    }
}