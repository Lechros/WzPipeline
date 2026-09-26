using Newtonsoft.Json;
using WzPipeline.Domains.Shared;

namespace WzPipeline.Domains.Soul;

public class MalibRawSoul
{
    [JsonIgnore] public required int Id { get; init; }
    public required string Name { get; init; }
    public bool Magnificent { get; init; }
    public GearOption? Option { get; set; }
    public GearOption[]? Options { get; set; }
}
