using WzPipeline.Core.Stereotype;

namespace WzPipeline.OldDomains.SubWeaponTransfer;

public interface ISubWeaponTransferNode : INode
{
    public int Job { get; }

    public IEnumerable<IEnumerable<int>> TargetIdGroups { get; }
}