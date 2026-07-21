using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.Core;
using WzPipeline.Application.Exporters;
using WzPipeline.Application.Pipelines;
using WzPipeline.Application.Shared.Export;
using WzPipeline.Application.Shared.Json;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Soul;
using WzPipeline.Wz;

namespace WzPipeline.Application.Configuration;

public static class ApplicationConfiguration
{
    public const string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<WzTree>(_ => WzTree.Load(BaseWzPath));
        services.AddSingleton<GearParser>();
        services.AddSingleton<ItemOptionParser>();
        services.AddSingleton<AstraSubWeaponParser>();
        services.AddSingleton<SetItemParser>();
        services.AddSingleton<ExclusiveEquipParser>();
        services.AddSingleton<SoulParser>();
        services.AddSingleton<SoulCollectionParser>();
        services.AddSingleton<SkillOptionParser>();
        services.AddApplicationJson();
        services.AddSingleton<JsonFileExporter>();
        services.AddSingleton<ImageFileExporter>();
        services.AddSingleton<JsonPipelineExporter>();
        services.AddSingleton<ImagePipelineExporter>();
        return services;
    }

    public static PipelineExportOptions CreateExportOptions()
    {
        return new PipelineExportOptions
        {
            OutputRootPath = Path.GetFullPath("output"),
            OutputPaths = new Dictionary<PipelineId, string>
            {
                [PipelineIds.GearData] = "gear.json",
                [PipelineIds.GearIcon] = "gear-icon",
                [PipelineIds.GearIconOrigin] = "gear-icon-origin.json",
                [PipelineIds.GearRawIcon] = "gear-raw-icon",
                [PipelineIds.GearRawIconOrigin] = "gear-raw-icon-origin.json",
                [PipelineIds.ItemIcon] = "item-icon",
                [PipelineIds.ItemIconOrigin] = "item-icon-origin.json",
                [PipelineIds.ItemRawIcon] = "item-raw-icon",
                [PipelineIds.ItemRawIconOrigin] = "item-raw-icon-origin.json",
                [PipelineIds.SetItemData] = "set-item.json",
                [PipelineIds.ExclusiveEquipData] = "exclusive-equip.json",
                [PipelineIds.SoulData] = "soul.json"
            }
        };
    }
}