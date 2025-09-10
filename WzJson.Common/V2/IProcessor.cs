namespace WzJson.Common.V2;

public interface IProcessor<in TIn, out TOut>
{
    public IEnumerable<TOut> Process(IEnumerable<TIn> models);
}