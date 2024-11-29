using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SoulStringNodeRepository : INodeRepository
{
    private const string StringConsumeNode = @"String\Consume.img";

    private readonly IWzProvider wzProvider;

    public SoulStringNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var stringConsumeNode = GetStringConsumeNode();
        foreach (var itemNode in stringConsumeNode.Nodes)
        {
            yield return itemNode;
        }

        stringConsumeNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetStringConsumeNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(StringConsumeNode, true)
               ?? throw new ApplicationException("Cannot find String Consume node at: " + StringConsumeNode);
    }
}