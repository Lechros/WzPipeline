using Jering.Javascript.NodeJS;
using Newtonsoft.Json;

namespace WzJson.Shared.Json;

public class JsonService(JsonSerializer serializer) : IJsonService
{
    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        return new ValueTask<T?>(Task.Run(() =>
        {
            using var sr = new StreamReader(stream, leaveOpen: true);
            using var jsonReader = new JsonTextReader(sr);
            return serializer.Deserialize<T>(jsonReader);
        }, cancellationToken));
    }

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var sw = new StreamWriter(stream, leaveOpen: true);
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.Formatting = serializer.Formatting;
            serializer.Serialize(jsonWriter, value);
            sw.Flush();
        }, cancellationToken);
    }
}