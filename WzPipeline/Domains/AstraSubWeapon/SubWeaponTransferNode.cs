using WzComparerR2.WzLib;
using WzPipeline.Domains.Shared;

namespace WzPipeline.Domains.AstraSubWeapon;

public class SubWeaponTransferNode(Wz_Node node)
{
    public Wz_Node Node => node;
    public string Id => node.Text;
    public int Job => int.Parse(Id);

    public IEnumerable<IEnumerable<int>> TargetIdGroups
    {
        get
        {
            var targetNode = node.Nodes["target"] ?? throw DataFormatException.MissingRequiredNode(node, "target");

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