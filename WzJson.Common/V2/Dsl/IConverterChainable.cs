namespace WzJson.Common.V2.Dsl;

public interface IConverterChainable<out TThis, TNextIn> where TNextIn : INode
{
    public IEnumerable<IConverterBuilder<TNextIn>> ChainedConverters { get; }

    public TThis Converter<TNextOut>(IConverter<TNextIn, TNextOut> next,
        Action<ConverterBuilder<TNextIn, TNextOut>> config);

    public TThis Converter<TNextOut>(Condition when,
        IConverter<TNextIn, TNextOut> next,
        Action<ConverterBuilder<TNextIn, TNextOut>> config);
}