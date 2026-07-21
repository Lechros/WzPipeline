using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.Core;
using WzPipeline.Application.Exporters;
using WzPipeline.Application.Pipelines;
using WzPipeline.Application.Sources;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Soul;
using WzPipeline.Wz;

namespace WzPipeline.Application.Configuration;

public static class PipelineRegistryFactory
{
    public static PipelineRegistry Create()
    {
        var registry = new PipelineRegistry();
        registry.Register<ItemOptionDataPipeline>(PipelineIds.ItemOptionData)
            .Create(ctx => new ItemOptionDataPipeline(
                ctx.Services.GetRequiredService<ItemOptionParser>(), ctx.CancellationToken))
            .Consumes(SourceIds.ItemOptionNodes, pipeline => pipeline.Input)
            .Produces(pipeline => pipeline.Result)
            .Exports((pipeline, context) => context.Services
                .GetRequiredService<JsonPipelineExporter>()
                .ExportAsync(pipeline.Result, context));

        registry.Register<GearStringDataPipeline>(PipelineIds.GearStringData)
            .Create(ctx => new GearStringDataPipeline(ctx.CancellationToken))
            .Consumes(SourceIds.GearStringNodes, pipeline => pipeline.Input)
            .Produces(pipeline => pipeline.Result);

        registry.Register<SkillNameDataPipeline>(PipelineIds.SkillNameData)
            .Create(ctx => new SkillNameDataPipeline(ctx.CancellationToken))
            .Consumes(SourceIds.SkillNameNodes, pipeline => pipeline.Input)
            .Produces(pipeline => pipeline.Result);

        registry.Register<AstraSubWeaponDataPipeline>(PipelineIds.AstraSubWeaponData)
            .Create(ctx => new AstraSubWeaponDataPipeline(
                ctx.Services.GetRequiredService<AstraSubWeaponParser>(), ctx.CancellationToken))
            .Consumes(SourceIds.AstraSubWeaponNodes, pipeline => pipeline.Input)
            .Produces(pipeline => pipeline.Result);

        registry.Register<GearDataPipeline>(PipelineIds.GearData)
            .DependsOn(
                PipelineIds.ItemOptionData,
                PipelineIds.GearStringData,
                PipelineIds.SkillNameData,
                PipelineIds.AstraSubWeaponData)
            .Create(ctx => new GearDataPipeline(
                ctx.Services.GetRequiredService<GearParser>(),
                ctx.GetRequiredPipeline<ItemOptionDataPipeline>(PipelineIds.ItemOptionData).Result,
                ctx.GetRequiredPipeline<GearStringDataPipeline>(PipelineIds.GearStringData).Result,
                ctx.GetRequiredPipeline<SkillNameDataPipeline>(PipelineIds.SkillNameData).Result,
                ctx.GetRequiredPipeline<AstraSubWeaponDataPipeline>(PipelineIds.AstraSubWeaponData).Result,
                ctx.CancellationToken))
            .Consumes(SourceIds.GearNodes, pipeline => pipeline.Input)
            .Produces(pipeline => pipeline.Result)
            .Exports((pipeline, context) => context.Services
                .GetRequiredService<JsonPipelineExporter>()
                .ExportAsync(pipeline.Result, context));

        registry.Register<GearIconPipeline>(PipelineIds.GearIcon)
            .Create(ctx => new GearIconPipeline(
                ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.GearNodes, pipeline => pipeline.Input)
            .ExportsStream(pipeline => pipeline.Output, (output, context) => context.Services
                .GetRequiredService<ImagePipelineExporter>()
                .AttachAsync(output, context));

        registry.Register<GearRawIconPipeline>(PipelineIds.GearRawIcon)
            .Create(ctx => new GearRawIconPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.GearNodes, p => p.Input)
            .ExportsStream(p => p.Output,
                (output, context) => context.Services.GetRequiredService<ImagePipelineExporter>()
                    .AttachAsync(output, context));

        registry.Register<GearIconOriginPipeline>(PipelineIds.GearIconOrigin)
            .Create(ctx => new GearIconOriginPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.GearNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<GearRawIconOriginPipeline>(PipelineIds.GearRawIconOrigin)
            .Create(ctx =>
                new GearRawIconOriginPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.GearNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<ItemIconPipeline>(PipelineIds.ItemIcon)
            .Create(ctx => new ItemIconPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.ItemNodes, p => p.Input)
            .ExportsStream(p => p.Output,
                (output, context) => context.Services.GetRequiredService<ImagePipelineExporter>()
                    .AttachAsync(output, context));

        registry.Register<ItemRawIconPipeline>(PipelineIds.ItemRawIcon)
            .Create(ctx => new ItemRawIconPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.ItemNodes, p => p.Input)
            .ExportsStream(p => p.Output,
                (output, context) => context.Services.GetRequiredService<ImagePipelineExporter>()
                    .AttachAsync(output, context));

        registry.Register<ItemIconOriginPipeline>(PipelineIds.ItemIconOrigin)
            .Create(ctx => new ItemIconOriginPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.ItemNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<ItemRawIconOriginPipeline>(PipelineIds.ItemRawIconOrigin)
            .Create(ctx =>
                new ItemRawIconOriginPipeline(ctx.Services.GetRequiredService<WzTree>(), ctx.CancellationToken))
            .Consumes(SourceIds.ItemNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<SetItemDataPipeline>(PipelineIds.SetItemData)
            .DependsOn(PipelineIds.ItemOptionData)
            .Create(ctx => new SetItemDataPipeline(ctx.Services.GetRequiredService<SetItemParser>(),
                ctx.GetRequiredPipeline<ItemOptionDataPipeline>(PipelineIds.ItemOptionData).Result,
                ctx.CancellationToken))
            .Consumes(SourceIds.SetItemNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<ExclusiveEquipDataPipeline>(PipelineIds.ExclusiveEquipData)
            .DependsOn(PipelineIds.GearStringData)
            .Create(ctx => new ExclusiveEquipDataPipeline(ctx.Services.GetRequiredService<ExclusiveEquipParser>(),
                ctx.GetRequiredPipeline<GearStringDataPipeline>(PipelineIds.GearStringData).Result,
                ctx.CancellationToken))
            .Consumes(SourceIds.ExclusiveEquipNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        registry.Register<ConsumeNameDataPipeline>(PipelineIds.ConsumeNameData)
            .Create(c => new ConsumeNameDataPipeline(c.CancellationToken))
            .Consumes(SourceIds.ConsumeNameNodes, p => p.Input).Produces(p => p.Result);

        registry.Register<SoulInfoDataPipeline>(PipelineIds.SoulInfoData)
            .Create(c =>
                new SoulInfoDataPipeline(c.Services.GetRequiredService<SoulCollectionParser>(), c.CancellationToken))
            .Consumes(SourceIds.SoulCollectionNodes, p => p.Input).Produces(p => p.Result);

        registry.Register<SkillOptionDataPipeline>(PipelineIds.SkillOptionData).DependsOn(PipelineIds.ItemOptionData)
            .Create(c => new SkillOptionDataPipeline(c.Services.GetRequiredService<SkillOptionParser>(),
                c.GetRequiredPipeline<ItemOptionDataPipeline>(PipelineIds.ItemOptionData).Result, c.CancellationToken))
            .Consumes(SourceIds.SkillOptionNodes, p => p.Input).Produces(p => p.Result);

        registry.Register<SoulDataPipeline>(PipelineIds.SoulData)
            .DependsOn(PipelineIds.ConsumeNameData, PipelineIds.SkillNameData, PipelineIds.SoulInfoData,
                PipelineIds.SkillOptionData)
            .Create(c => new SoulDataPipeline(c.Services.GetRequiredService<SoulParser>(),
                c.GetRequiredPipeline<ConsumeNameDataPipeline>(PipelineIds.ConsumeNameData).Result,
                c.GetRequiredPipeline<SkillNameDataPipeline>(PipelineIds.SkillNameData).Result,
                c.GetRequiredPipeline<SoulInfoDataPipeline>(PipelineIds.SoulInfoData).Result,
                c.GetRequiredPipeline<SkillOptionDataPipeline>(PipelineIds.SkillOptionData).Result,
                c.CancellationToken)).Consumes(SourceIds.SoulNodes, p => p.Input).Produces(p => p.Result)
            .Exports((p, c) => c.Services.GetRequiredService<JsonPipelineExporter>().ExportAsync(p.Result, c));

        return registry;
    }
}