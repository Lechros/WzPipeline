using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class GearNodeRepository : INodeRepository
{
    private const string CharacterNodePath = "Character";
    private const string CanvasNodeText = "_Canvas";

    private static readonly ISet<string> PartNames = new HashSet<string>
    {
        "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove", "Longcoat", "Mechanic", "Pants", "Ring",
        "Shield", "Shoes", "Weapon"
    };

    private readonly IWzProvider wzProvider;
    private readonly Wz_Node characterNode;

    public GearNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
        characterNode = GetCharacterNode();
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var partNode in characterNode.Nodes)
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

    private Wz_Node GetCharacterNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(CharacterNodePath)
               ?? throw new ApplicationException("Cannot find Character node at: " + CharacterNodePath);
    }
}