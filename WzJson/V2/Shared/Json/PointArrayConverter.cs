using System.Drawing;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace WzJson.V2.Shared.Json;

public class PointArrayConverter : JsonConverter<Point>
{
    public override void WriteJson(JsonWriter writer, Point value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.X);
        writer.WriteValue(value.Y);
        writer.WriteEndArray();
    }

    public override Point ReadJson(JsonReader reader, Type objectType, Point existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        reader.Read();
        int x = Convert.ToInt32(reader.Value);

        reader.Read();
        int y = Convert.ToInt32(reader.Value);

        reader.Read(); // EndArray

        return new Point(x, y);
    }
}