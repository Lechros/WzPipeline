using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Processor;

public static class ProcessorChain
{
    public static IProcessor<T1, TResult> Of<T1, T2, TResult>(IProcessor<T1, T2> processor1,
        IProcessor<T2, TResult> processor2)
    {
        return new Chain<T1, TResult>((IProcessor)processor1);
    }

    public static IProcessor<T1, TResult> Of<T1, T2, T3, TResult>(
        IProcessor<T1, T2> processor1,
        IProcessor<T2, T3> processor2,
        IProcessor<T3, TResult> processor3)
    {
        return new Chain<T1, TResult>((IProcessor)processor1, (IProcessor)processor2,
            (IProcessor)processor3);
    }

    public static IProcessor<T1, TResult> Of<T1, T2, T3, T4, TResult>(
        IProcessor<T1, T2> processor1,
        IProcessor<T2, T3> processor2,
        IProcessor<T3, T4> processor3,
        IProcessor<T4, TResult> processor4)
    {
        return new Chain<T1, TResult>((IProcessor)processor1, (IProcessor)processor2,
            (IProcessor)processor3, (IProcessor)processor4);
    }

    public static IProcessor<T1, TResult> Of<T1, T2, T3, T4, T5, TResult>(
        IProcessor<T1, T2> processor1,
        IProcessor<T2, T3> processor2,
        IProcessor<T3, T4> processor3,
        IProcessor<T4, T5> processor4,
        IProcessor<T5, TResult> processor5)
    {
        return new Chain<T1, TResult>((IProcessor)processor1, (IProcessor)processor2,
            (IProcessor)processor3, (IProcessor)processor4, (IProcessor)processor5);
    }

    public static IProcessor<T1, TResult> Of<T1, T2, T3, T4, T5, T6, TResult>(
        IProcessor<T1, T2> processor1,
        IProcessor<T2, T3> processor2,
        IProcessor<T3, T4> processor3,
        IProcessor<T4, T5> processor4,
        IProcessor<T5, T6> processor5,
        IProcessor<T6, TResult> processor6)
    {
        return new Chain<T1, TResult>((IProcessor)processor1, (IProcessor)processor2,
            (IProcessor)processor3, (IProcessor)processor4, (IProcessor)processor5, (IProcessor)processor6);
    }

    public static IProcessor<T1, TResult> Of<T1, T2, T3, T4, T5, T6, T7, TResult>(
        IProcessor<T1, T2> processor1,
        IProcessor<T2, T3> processor2,
        IProcessor<T3, T4> processor3,
        IProcessor<T4, T5> processor4,
        IProcessor<T5, T6> processor5,
        IProcessor<T6, T7> processor6,
        IProcessor<T7, TResult> processor7)
    {
        return new Chain<T1, TResult>((IProcessor)processor1, (IProcessor)processor2,
            (IProcessor)processor3, (IProcessor)processor4, (IProcessor)processor5, (IProcessor)processor6,
            (IProcessor)processor7);
    }

    private class Chain<TIn, TOut> : AbstractProcessor<TIn, TOut>
    {
        private readonly IProcessor[] _processors;

        internal Chain(params IProcessor[] processors)
        {
            _processors = processors;
        }

        public override IEnumerable<TOut> Process(IEnumerable<TIn> models)
        {
            return _processors
                .Aggregate(
                    models.Cast<object>(),
                    (current, processor) => processor.Process(current))
                .Cast<TOut>();
        }
    }
}