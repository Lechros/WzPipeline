namespace WzJson.V2.Core.Stereotype;

public interface IProcessor
{
    public IEnumerable<object> Process(IEnumerable<object> models);
}

public interface IProcessor<in TIn, out TOut>
{
    public IEnumerable<TOut> Process(IEnumerable<TIn> models);
}