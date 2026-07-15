namespace WzPipeline.Domains.Soul;

public class SoulCollectionParser
{
    public IEnumerable<SoulInfo> Parse(SoulCollectionNode node)
    {
        for (var i = 0; i < node.SoulList.Length; i++)
        {
            var soulId = node.SoulList[i][0]; // Pick first soul which is first soul introduced in game
            var skillId = SoulInfo.IsMagnificentIndex(i) ? node.SoulSkillH!.Value : node.SoulSkill;
            yield return new SoulInfo
            {
                SoulId = soulId,
                SkillId = skillId,
                Index = i,
            };
        }
    }
}