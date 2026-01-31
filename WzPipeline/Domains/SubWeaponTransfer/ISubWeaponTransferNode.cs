using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.SubWeaponTransfer;

public interface ISubWeaponTransferNode : INode
{
    public int Job { get; }

    public IEnumerable<IEnumerable<int>> TargetIdGroups { get; }
}