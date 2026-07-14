using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Domains.ExclusiveEquip;

public class ExclusiveEquipParseContext
{
    public required IReadOnlyDictionary<string, NameDesc> GearStringData { get; init; }
}