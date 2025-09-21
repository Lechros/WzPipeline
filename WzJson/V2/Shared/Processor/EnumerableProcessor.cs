using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Processor;

public class EnumerableProcessor<TIn, TOut>(Func<IEnumerable<TIn>, IEnumerable<TOut>> processor)
    : AbstractProcessor<TIn, TOut>
{
    public override IEnumerable<TOut> Process(IEnumerable<TIn> models)
    {
        return processor(models);
    }
}

public static class EnumerableProcessor
{
    public static EnumerableProcessor<TIn, TOut> Create<TIn, TOut>(Func<IEnumerable<TIn>, IEnumerable<TOut>> func)
    {
        return new EnumerableProcessor<TIn, TOut>(func);
    }
}