using WzPipeline.Domains.Gear.Models;

namespace WzPipeline.Domains.ItemOption;

public class ItemOptionEntry
{
    public required int Code { get; init; }
    public int? OptionType { get; init; }
    public int? ReqLevel { get; init; }
    public SortedDictionary<int, LevelOption> Level { get; } = new();
}

public class LevelOption
{
    public required string String { get; set; }
    public GearOption Option { get; set; } = new();
}