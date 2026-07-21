using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Application.Core;
using WzPipeline.Application.Sources;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Item;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Domains.Soul;
using WzPipeline.Wz;

namespace WzPipeline.Application.Configuration;

public static class SourceRegistryFactory
{
    public static SourceRegistry Create()
    {
        var registry = new SourceRegistry();

        registry.Register(SourceIds.ItemOptionNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.ItemOption)
                .Select(n => new ItemOptionNode(n)));

        registry.Register(SourceIds.GearNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.Gear).Select(n => new GearNode(n))
                .Where(n => n.Id is not null));

        registry.Register(SourceIds.ItemNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.Item).Select(n => new ItemNode(n)));

        registry.Register(SourceIds.SetItemNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.SetItem).Select(n => new SetItemNode(n)));

        registry.Register(SourceIds.ExclusiveEquipNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.ExclusiveEquip)
                .Select(n => new ExclusiveEquipNode(n)));

        registry.Register(SourceIds.ConsumeNameNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.ConsumeName)
                .Select(n => new StringNode(n)));

        registry.Register(SourceIds.SoulNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.Soul).Select(n => new SoulNode(n)));

        registry.Register(SourceIds.SoulCollectionNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.SoulCollection)
                .Select(n => new SoulCollectionNode(n)));

        registry.Register(SourceIds.SkillOptionNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.SkillOption)
                .Select(n => new SkillOptionNode(n)));

        registry.Register(SourceIds.GearStringNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.GearString)
                .Select(n => new StringNode(n)));

        registry.Register(SourceIds.SkillNameNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.SkillName)
                .Select(n => new StringNode(n)));

        registry.Register(SourceIds.AstraSubWeaponNodes,
            s => s.GetRequiredService<WzTree>().MatchNodes(SourcePaths.AstraSubWeapon)
                .Select(n => new SubWeaponTransferNode(n)));

        return registry;
    }
}