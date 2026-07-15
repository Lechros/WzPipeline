using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Shared;

namespace WzPipeline.Application.ExclusiveEquip;

public class ExclusiveEquipDataProvider(ExclusiveEquipDataBuilder builder)
    : AsyncDataProvider<SortedDictionary<int, MalibExclusiveEquip>>
{
    protected override Task<SortedDictionary<int, MalibExclusiveEquip>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}