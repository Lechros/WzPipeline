namespace WzJson.V2.Pipeline.Abstractions;

public abstract class AbstractProcessor<TIn, TOut> : IProcessor<TIn, TOut>, IProcessor
{
    public abstract IEnumerable<TOut> Process(IEnumerable<TIn> models);

    public IEnumerable<object> Process(IEnumerable<object> models)
    {
        return Process(models.Cast<TIn>()).Cast<object>();
    }
}