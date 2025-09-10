namespace WzJson.Common.V2.Dsl;

public interface IProcessorBuilder<in TIn>
{
    public IProcessor<TIn, object> Get();
}