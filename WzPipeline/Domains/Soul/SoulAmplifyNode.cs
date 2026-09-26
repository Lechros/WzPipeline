using Wz;

namespace WzPipeline.Domains.Soul;

public class SoulAmplifyNode(IWzNode node)
{
    public int[] UpgradableSoul => node.FindRequiredNode("upgradableSoul", WzNodeFindOptions.LoadImage).Nodes
        .Select(n => n.GetInt32()).ToArray();
}
