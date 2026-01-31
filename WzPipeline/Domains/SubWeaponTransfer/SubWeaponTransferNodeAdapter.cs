using WzComparerR2.WzLib;

namespace WzPipeline.Domains.SubWeaponTransfer;

public class SubWeaponTransferNodeAdapter(Wz_Node node) : ISubWeaponTransferNode
{
    public static SubWeaponTransferNodeAdapter Create(Wz_Node node)
    {
        return new SubWeaponTransferNodeAdapter(node);
    }

    public string Id => node.Text;

    public int Job => int.Parse(Id);

    public IEnumerable<IEnumerable<int>> TargetIdGroups
    {
        get
        {
            var targetNode = node.Nodes["target"];
            var shieldNode = targetNode.FindNodeByPath("shield");
            if (shieldNode != null)
            {
                yield return shieldNode.Nodes.Select(n => int.Parse(n.Text));
            }

            var nonshieldNode = targetNode.FindNodeByPath("nonshield");
            if (nonshieldNode != null)
            {
                yield return nonshieldNode.Nodes.Select(n => int.Parse(n.Text));
            }

            if (shieldNode == null && nonshieldNode == null)
            {
                yield return targetNode.Nodes.Select(n => int.Parse(n.Text));
            }
        }
    }
}