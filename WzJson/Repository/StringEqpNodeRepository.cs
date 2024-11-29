using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class StringEqpNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    private static readonly HashSet<string> PartNames =
    [
        "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove", "Longcoat", "Mechanic", "Pants", "Ring",
        "Shield", "Shoes", "Weapon"
    ];

    protected override string RootNodePath => @"String\Eqp.img\Eqp";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var partNode in rootNode.Nodes)
        {
            if (!PartNames.Contains(partNode.Text)) continue;
            
            foreach (var gearNode in partNode.Nodes)
            {
                yield return gearNode;
            }
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}