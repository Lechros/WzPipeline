using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.DataBuilders;
using WzPipeline.Application.DataProviders;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Wz;

namespace WzPipeline.Application;

public class Bootstrap
{
    private const string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // WzTree
        services.AddSingleton<WzTree>(_ => WzTree.Load(BaseWzPath));

        AddItemOptionDataProvider(services);
        AddSetItemDataProvider(services);
        AddAstraSubWeaponDataProvider(services);
        AddSkillNameDataProvider(services);
        AddGearStringDataProvider(services);
        AddGearDataProvider(services);
        AddExclusiveEquipDataProvider(services);

        await using var provider = services.BuildServiceProvider();
        var sw = new Stopwatch();

        Console.WriteLine($"Loading Base.wz: {BaseWzPath}");
        sw.Restart();
        provider.GetRequiredService<WzTree>();
        sw.Stop();
        Console.WriteLine($"Loaded Base.wz in {sw.ElapsedMilliseconds}ms");

        Console.WriteLine("Loading SetItem");
        sw.Restart();
        var setItemDataProvider = provider.GetRequiredService<SetItemDataProvider>();
        var setItemData = await setItemDataProvider.GetAsync();
        sw.Stop();
        Console.WriteLine($"Loaded SetItem({setItemData.Count}) in {sw.ElapsedMilliseconds}ms");

        Console.WriteLine("Loading ExclusiveEquip");
        sw.Restart();
        var exclusiveEquipDataProvider = provider.GetRequiredService<ExclusiveEquipDataProvider>();
        var exclusiveEquipData = await exclusiveEquipDataProvider.GetAsync();
        sw.Stop();
        Console.WriteLine($"Loaded ExclusiveEquip({exclusiveEquipData.Count}) in {sw.ElapsedMilliseconds}ms");

        // Console.WriteLine("Loading Gear");
        // sw.Restart();
        // var gearDataProvider = provider.GetRequiredService<GearDataProvider>();
        // var gearData = await gearDataProvider.GetAsync();
        // sw.Stop();
        // Console.WriteLine($"Loaded Gear({gearData.Count}) in {sw.ElapsedMilliseconds}ms");
    }

    static void AddItemOptionDataProvider(IServiceCollection services)
    {
        services.AddSingleton<ItemOptionParser>();
        services.AddSingleton<ItemOptionDataBuilder>();
        services.AddSingleton<ItemOptionDataProvider>();
    }

    static void AddSetItemDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SetItemParser>();
        services.AddSingleton<SetItemDataBuilder>();
        services.AddSingleton<SetItemDataProvider>();
    }

    static void AddAstraSubWeaponDataProvider(IServiceCollection services)
    {
        services.AddSingleton<AstraSubWeaponParser>();
        services.AddSingleton<AstraSubWeaponDataBuilder>();
        services.AddSingleton<AstraSubWeaponDataProvider>();
    }

    static void AddSkillNameDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SkillNameDataBuilder>();
        services.AddSingleton<SkillNameDataProvider>();
    }

    static void AddGearStringDataProvider(IServiceCollection services)
    {
        services.AddSingleton<GearStringDataBuilder>();
        services.AddSingleton<GearStringDataProvider>();
    }

    static void AddGearDataProvider(IServiceCollection services)
    {
        services.AddSingleton<GearParser>();
        services.AddSingleton<GearDataBuilder>();
        services.AddSingleton<GearDataProvider>();
    }
    
    static void AddExclusiveEquipDataProvider(IServiceCollection services)
    {
        services.AddSingleton<ExclusiveEquipParser>();
        services.AddSingleton<ExclusiveEquipDataBuilder>();
        services.AddSingleton<ExclusiveEquipDataProvider>();
    }
}