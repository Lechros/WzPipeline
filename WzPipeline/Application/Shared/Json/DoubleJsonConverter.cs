using System.Numerics;
using Newtonsoft.Json;

namespace WzPipeline.Application.Shared.Json;

public sealed class DoubleJsonConverter : JsonConverter<double>
{
    public override bool CanRead => false;

    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        if (double.IsFinite(value) && value == Math.Truncate(value))
            writer.WriteValue(new BigInteger(value));
        else
            writer.WriteValue(value);
    }

    public override double ReadJson(JsonReader reader, Type objectType, double existingValue,
        bool hasExistingValue, JsonSerializer serializer) => throw new NotSupportedException();
}
