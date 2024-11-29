using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Gear;

public class StringEqpNodeRepository : INodeRepository
{
    private const string StringEqpNodePath = @"String\Eqp.img\Eqp";

    private static readonly ISet<string> PartNames = new HashSet<string>
    {
        "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove", "Longcoat", "Mechanic", "Pants", "Ring",
        "Shield", "Shoes", "Weapon"
    };

    private readonly IWzProvider wzProvider;

    public StringEqpNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var stringEqpNode = GetStringEqpNode();
        foreach (var partNode in stringEqpNode.Nodes)
        {
            if (!PartNames.Contains(partNode.Text)) continue;
            foreach (var gearNode in partNode.Nodes)
            {
                yield return gearNode;
            }
        }

        stringEqpNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetStringEqpNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(StringEqpNodePath, true)
               ?? throw new ApplicationException("Cannot find String Eqp node at: " + StringEqpNodePath);
    }
}