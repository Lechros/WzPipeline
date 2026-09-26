namespace WzPipeline.Domains.Soul;

public class SoulCollectionParser
{
    public IEnumerable<SoulInfo> Parse(SoulCollectionNode node)
    {
        return node.SoulList.Select(t => new SoulInfo
        {
            SoulIds = t,
        });
    }
}