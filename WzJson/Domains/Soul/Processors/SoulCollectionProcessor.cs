using WzJson.Core.Stereotype;
using WzJson.Domains.Soul.Models;

namespace WzJson.Domains.Soul.Processors;

public class SoulCollectionProcessor : AbstractProcessor<SoulCollection, SoulInfo>
{
    private const int MagnificentIndex = 8;

    public override IEnumerable<SoulInfo> Process(IEnumerable<SoulCollection> souls)
    {
        foreach (var soul in souls)
        {
            for (var i = 0; i < soul.SoulList.Length; i++)
            {
                var isMagnificent = i == MagnificentIndex;
                var soulIds = soul.SoulList[i];
                var soulId = soulIds[0]; // Pick first soul (TradeBlock)
                var skillId = isMagnificent ? soul.SoulSkillH!.Value : soul.SoulSkill;
                yield return new SoulInfo
                {
                    SoulId = soulId,
                    SkillId = skillId,
                    Index = i,
                };
            }
        }
    }
}