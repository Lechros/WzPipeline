using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class GearNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    private const string CanvasNodeText = "_Canvas";

    private static readonly HashSet<string> PartNames =
    [
        "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove", "Longcoat", "Mechanic", "Pants", "Ring",
        "Shield", "Shoes", "Weapon"
    ];

    protected override string RootNodePath => "Character";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var partNode in GetRootNode().Nodes)
        {
            if (!PartNames.Contains(partNode.Text)) continue;
            
            foreach (var gearNode in partNode.Nodes)
            {
                if (gearNode.Text == CanvasNodeText) continue;

                var wzImage = gearNode.GetNodeWzImage();
                if (wzImage == null || !wzImage.TryExtract()) continue;

                yield return wzImage.Node;

                wzImage.Unextract();
            }
        }
    }
}