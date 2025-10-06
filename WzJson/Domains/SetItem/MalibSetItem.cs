using Newtonsoft.Json;
using WzJson.Domains.Gear.Models;

namespace WzJson.Domains.SetItem;

public class MalibSetItem
{
    [JsonIgnore] public required int Id { get; init; }
    public required string Name { get; init; }
    public required int[] ItemIds { get; init; }
    public required SortedDictionary<int, GearOption> Effects { get; init; }
    public bool JokerPossible { get; init; }
    public bool ZeroWeaponJokerPossible { get; init; }

    public bool ShouldSerializeItems() => ItemIds.Length > 0;
    public bool ShouldSerializeEffect() => Effects.Count > 0;
}