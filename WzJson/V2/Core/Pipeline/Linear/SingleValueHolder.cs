using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline.Linear;

public class SingleValueHolder<T> : AbstractExporter<T>
{
    private T? _value;

    public bool HasValue { get; private set; }

    public T Value
    {
        get => HasValue ? _value! : throw new InvalidOperationException("HasValue is false.");
        private set
        {
            _value = value;
            HasValue = true;
        }
    }

    public void ClearValue()
    {
        _value = default;
        HasValue = false;
    }

    protected override void Prepare()
    {
    }

    public override Task Export(T model)
    {
        if (HasValue)
        {
            throw new InvalidOperationException("SingleCollector only accepts single value.");
        }

        Value = model;
        return Task.CompletedTask;
    }
}