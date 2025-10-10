using Newtonsoft.Json;

namespace WzPipeline.Domains.ExclusiveEquip;

public class MalibExclusiveEquip
{
    [JsonIgnore] public required int Id { get; init; }
    public required int[] ItemIds { get; init; }
    public required string[] Names { get; init; }
}