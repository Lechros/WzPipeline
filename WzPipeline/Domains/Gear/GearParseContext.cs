using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Domains.Gear;

public class GearParseContext
{
    public required IReadOnlyDictionary<string, NameDesc> GearStringData { get; init; }
    public required ItemOptionData ItemOptionData { get; init; }
    public required IReadOnlyDictionary<string, string> SkillNameData { get; init; }
    public required AstraSubWeaponData AstraSubWeaponData { get; init; }
}