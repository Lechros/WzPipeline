namespace WzJson.V2.Pipeline.Abstractions;

public interface IProcessor
{
    public IEnumerable<object> Process(IEnumerable<object> models);
}

public interface IProcessor<in TIn, out TOut>
{
    public IEnumerable<TOut> Process(IEnumerable<TIn> models);
}