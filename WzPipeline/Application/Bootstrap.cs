using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.ExclusiveEquip;
using WzPipeline.Application.Gear;
using WzPipeline.Application.Item;
using WzPipeline.Application.ItemOption;
using WzPipeline.Application.SetItem;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Application.Shared.Export;
using WzPipeline.Application.Skill;
using WzPipeline.Application.Soul;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Soul;
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
        AddSoulInfoDataProvider(services);
        AddSkillOptionDataProvider(services);
        AddConsumeNameDataProvider(services);
        AddSoulDataProvider(services);
        services.AddSingleton<GearIconExporter>();

        await using var provider = services.BuildServiceProvider();

        var sw = Stopwatch.StartNew();

        var tree = provider.GetRequiredService<WzTree>();
        var gearDataBuilder = provider.GetRequiredService<GearDataBuilder>();
        var gearIconExporter = provider.GetRequiredService<GearIconExporter>();
        var source = gearDataBuilder.CreateSourceBlock();
        var broadcaster = new BroadcastBlock<GearNode>(node => node);
        
        var parserBuffer = new BufferBlock<GearNode>();
        var parser = await gearDataBuilder.CreateParserBlockAsync();
        var collector = gearDataBuilder.CreateDictionaryCollector();
        
        var originConverterBuffer = new BufferBlock<GearNode>();
        var originConverter = gearIconExporter.CreateIconOriginConverterBlock(tree.FindNode);
        var originCollector = gearIconExporter.CreateOriginDictionaryCollector();
        
        var imageConverterBuffer= new BufferBlock<GearNode>();
        var imageConverter = gearIconExporter.CreateIconImageConverterBlock(tree.FindNode);
        var imageFileExporter = new ImageFileExporter();
        var imageExporter = DataflowExporters.ImageExporter(imageFileExporter, "tempIcon");
        
        source.LinkTo(broadcaster, new DataflowLinkOptions { PropagateCompletion = true });

        broadcaster.LinkTo(parserBuffer, new DataflowLinkOptions { PropagateCompletion = true });
        parserBuffer.LinkTo(parser, new DataflowLinkOptions { PropagateCompletion = true });
        parser.LinkTo(collector.Target, new DataflowLinkOptions { PropagateCompletion = true });
        
        broadcaster.LinkTo(originConverterBuffer, new DataflowLinkOptions { PropagateCompletion = true });
        originConverterBuffer.LinkTo(originConverter, new DataflowLinkOptions { PropagateCompletion = true });
        originConverter.LinkTo(originCollector.Target, new DataflowLinkOptions { PropagateCompletion = true });
        
        broadcaster.LinkTo(imageConverterBuffer, new DataflowLinkOptions { PropagateCompletion = true });
        imageConverterBuffer.LinkTo(imageConverter, new DataflowLinkOptions { PropagateCompletion = true });
        imageConverter.LinkTo(imageExporter, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        await originCollector.Completion;
        
        sw.Stop();
        
        Console.WriteLine($"Elapsed ms: {sw.ElapsedMilliseconds}");
        Console.WriteLine(collector.Result.Count);
        Console.WriteLine(originCollector.Result.Count);
        
        sw.Restart();

        await imageExporter.Completion;
        
        sw.Stop();
        
        Console.WriteLine($"All icon files written in {sw.ElapsedMilliseconds}ms after processing done");
        
        

        var nodes = provider.GetRequiredService<WzTree>().MatchNodes("Item/SkillOption.img").ToList();

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

        Console.WriteLine("Loading Soul");
        sw.Restart();
        var soulDataProvider = provider.GetRequiredService<SoulDataProvider>();
        var soulData = await soulDataProvider.GetAsync();
        sw.Stop();
        Console.WriteLine($"Loaded Soul({soulData.Count}) in {sw.ElapsedMilliseconds}ms");

        // Console.WriteLine("Loading Gear");
        // sw.Restart();
        // var gearDataProvider = provider.GetRequiredService<GearDataProvider>();
        // var gearData = await gearDataProvider.GetAsync();
        // sw.Stop();
        // Console.WriteLine($"Loaded Gear({gearData.Count}) in {sw.ElapsedMilliseconds}ms");
    }

    private static void AddItemOptionDataProvider(IServiceCollection services)
    {
        services.AddSingleton<ItemOptionParser>();
        services.AddSingleton<ItemOptionDataBuilder>();
        services.AddSingleton<ItemOptionDataProvider>();
    }

    private static void AddSetItemDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SetItemParser>();
        services.AddSingleton<SetItemDataBuilder>();
        services.AddSingleton<SetItemDataProvider>();
    }

    private static void AddAstraSubWeaponDataProvider(IServiceCollection services)
    {
        services.AddSingleton<AstraSubWeaponParser>();
        services.AddSingleton<AstraSubWeaponDataBuilder>();
        services.AddSingleton<AstraSubWeaponDataProvider>();
    }

    private static void AddSkillNameDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SkillNameDataBuilder>();
        services.AddSingleton<SkillNameDataProvider>();
    }

    private static void AddGearStringDataProvider(IServiceCollection services)
    {
        services.AddSingleton<GearStringDataBuilder>();
        services.AddSingleton<GearStringDataProvider>();
    }

    private static void AddGearDataProvider(IServiceCollection services)
    {
        services.AddSingleton<GearParser>();
        services.AddSingleton<GearDataBuilder>();
        services.AddSingleton<GearDataProvider>();
    }

    private static void AddExclusiveEquipDataProvider(IServiceCollection services)
    {
        services.AddSingleton<ExclusiveEquipParser>();
        services.AddSingleton<ExclusiveEquipDataBuilder>();
        services.AddSingleton<ExclusiveEquipDataProvider>();
    }

    private static void AddSoulInfoDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SoulCollectionParser>();
        services.AddSingleton<SoulInfoDataBuilder>();
        services.AddSingleton<SoulInfoDataProvider>();
    }

    private static void AddSkillOptionDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SkillOptionParser>();
        services.AddSingleton<SkillOptionDataBuilder>();
        services.AddSingleton<SkillOptionDataProvider>();
    }

    private static void AddConsumeNameDataProvider(IServiceCollection services)
    {
        services.AddSingleton<ConsumeNameDataBuilder>();
        services.AddSingleton<ConsumeNameDataProvider>();
    }

    private static void AddSoulDataProvider(IServiceCollection services)
    {
        services.AddSingleton<SoulParser>();
        services.AddSingleton<SoulDataBuilder>();
        services.AddSingleton<SoulDataProvider>();
    }
}