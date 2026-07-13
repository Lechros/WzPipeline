using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.DataProviders;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Skill;
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

        // Block Factory
        services.AddSingleton<ItemOptionBlockFactory>();
        services.AddSingleton<SetItemDataBlockFactory>();
        services.AddSingleton<AstraSubWeaponBlockFactory>();
        services.AddSingleton<GearStringBlockFactory>();
        services.AddSingleton<GearDataBlockFactory>();
        services.AddSingleton<SkillNameDataBlockFactory>();

        // Data Provider
        services.AddSingleton<ItemOptionDataProvider>();
        services.AddSingleton<SetItemDataProvider>();
        services.AddSingleton<AstraSubWeaponDataProvider>();
        services.AddSingleton<GearStringDataProvider>();
        services.AddSingleton<GearDataProvider>();
        services.AddSingleton<SkillNameDataProvider>();

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

        Console.WriteLine("Loading Gear");
        sw.Restart();
        var gearDataProvider = provider.GetRequiredService<GearDataProvider>();
        var gearData = await gearDataProvider.GetAsync();
        sw.Stop();
        Console.WriteLine($"Loaded Gear({gearData.Count}) in {sw.ElapsedMilliseconds}ms");
    }
}