using System.Threading.Tasks.Dataflow;

namespace WzPipeline.Application.Shared.Dataflow;

public class DataflowCollector<TInput, TResult>
{
    public required ITargetBlock<TInput> Target { get; init; }
    public required Task Completion { get; init; }
    public required TResult Result { get; init; }
}