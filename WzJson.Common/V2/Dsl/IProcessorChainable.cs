namespace WzJson.Common.V2.Dsl;

public interface IProcessorChainable<out TThis, TNextIn>
{
    public IEnumerable<IProcessorBuilder<TNextIn>> ChainedProcessors { get; }

    public TThis Processor<TNextOut>(IProcessor<TNextIn, TNextOut> next,
        Action<ProcessorBuilder<TNextIn, TNextOut>> config);

    public TThis Processor<TNextOut>(Condition when,
        IProcessor<TNextIn, TNextOut> next,
        Action<ProcessorBuilder<TNextIn, TNextOut>> config);
}