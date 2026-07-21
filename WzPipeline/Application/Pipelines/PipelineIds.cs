using WzPipeline.Application.Core;

namespace WzPipeline.Application.Pipelines;

public static class PipelineIds
{
    public static readonly PipelineId GearData = new("GearData");
    public static readonly PipelineId GearIcon = new("GearIcon");
    public static readonly PipelineId GearIconOrigin = new("GearIconOrigin");
    public static readonly PipelineId GearRawIcon = new("GearRawIcon");
    public static readonly PipelineId GearRawIconOrigin = new("GearRawIconOrigin");
    public static readonly PipelineId ItemIcon = new("ItemIcon");
    public static readonly PipelineId ItemIconOrigin = new("ItemIconOrigin");
    public static readonly PipelineId ItemRawIcon = new("ItemRawIcon");
    public static readonly PipelineId ItemRawIconOrigin = new("ItemRawIconOrigin");
    public static readonly PipelineId SetItemData = new("SetItemData");
    public static readonly PipelineId ExclusiveEquipData = new("ExclusiveEquipData");
    public static readonly PipelineId SoulData = new("SoulData");
    public static readonly PipelineId ConsumeNameData = new("ConsumeNameData");
    public static readonly PipelineId SoulInfoData = new("SoulInfoData");
    public static readonly PipelineId SkillOptionData = new("SkillOptionData");
    public static readonly PipelineId ItemOptionData = new("ItemOptionData");
    public static readonly PipelineId GearStringData = new("GearStringData");
    public static readonly PipelineId SkillNameData = new("SkillNameData");
    public static readonly PipelineId AstraSubWeaponData = new("AstraSubWeaponData");
}