using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class ExclusiveEquipDataProvider(ExclusiveEquipDataBuilder builder)
    : AsyncDataProvider<SortedDictionary<int, MalibExclusiveEquip>>
{
    protected override Task<SortedDictionary<int, MalibExclusiveEquip>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}