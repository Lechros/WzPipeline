using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SoulNodeRepository : INodeRepository
{
    private const string SoulNodePath = @"Item\Consume\0259.img";

    private readonly IWzProvider wzProvider;

    public SoulNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var soulRootNode = GetSoulNode();
        foreach (var soulNode in soulRootNode.Nodes)
        {
            yield return soulNode;
        }

        soulRootNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetSoulNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(SoulNodePath, true)
               ?? throw new ApplicationException("Cannot find Soul node at: " + SoulNodePath);
    }
}