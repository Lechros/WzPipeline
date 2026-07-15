using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Shared;

namespace WzPipeline.Application.Gear;

public class AstraSubWeaponDataProvider(AstraSubWeaponDataBuilder builder) : AsyncDataProvider<AstraSubWeaponData>
{
    protected override Task<AstraSubWeaponData> CreateAsync()
    {
        return builder.BuildAsync();
    }
}