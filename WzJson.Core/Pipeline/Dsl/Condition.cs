namespace WzJson.Core.Pipeline.Dsl;

public readonly struct Condition
{
    internal Condition(bool value)
    {
        Value = value;
    }

    public bool Value { get; }
}

public static class ConditionDsl
{
    public static Condition When(bool value) => new(value);
}