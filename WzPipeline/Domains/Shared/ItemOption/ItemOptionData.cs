namespace WzPipeline.Domains.Shared.ItemOption;

public class ItemOptionData : Dictionary<int, ItemOptionEntry>
{
    public GearOption GetGearOption(int optionCode, int level)
    {
        if (!TryGetValue(optionCode, out var entry) || !entry.Level.TryGetValue(level, out var option))
            throw new ItemOptionNotFoundException(optionCode, level);

        return option.Option;
    }
}

public class ItemOptionEntry
{
    public required int Code { get; init; }
    public int? OptionType { get; init; }
    public int? ReqLevel { get; init; }
    public SortedDictionary<int, LevelOption> Level { get; init; } = new();
}

public class LevelOption
{
    public required string String { get; set; }
    public GearOption Option { get; set; } = new();
    public int? FixedGrade { get; set; }
}