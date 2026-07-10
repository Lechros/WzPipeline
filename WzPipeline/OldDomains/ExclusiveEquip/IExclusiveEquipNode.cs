using WzPipeline.Core.Stereotype;

namespace WzPipeline.OldDomains.ExclusiveEquip;

public interface IExclusiveEquipNode : INode
{
    public string? Info { get; }
    public int[] ItemIds { get; }
    public string? Msg { get; }
}