using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class AstraSubWeaponDataProvider(AstraSubWeaponDataBuilder builder) : AsyncDataProvider<AstraSubWeaponData>
{
    protected override Task<AstraSubWeaponData> CreateAsync()
    {
        return builder.BuildAsync();
    }
}