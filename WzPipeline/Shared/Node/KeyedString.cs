using Newtonsoft.Json.Linq;

namespace WzPipeline.Shared.Node;

public class KeyedString
{
    public required string Key { get; init; }
    public required string Value { get; init; }
}