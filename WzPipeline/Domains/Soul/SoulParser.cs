using WzPipeline.Domains.Shared;

namespace WzPipeline.Domains.Soul;

public class SoulParser
{
    public IEnumerable<MalibSoul> Parse(SoulNode node, SoulParseContext context)
    {
        if (!int.TryParse(node.Id, out var id))
        {
            yield break;
        }

        if (!context.ConsumeNameData.TryGetValue(node.Id, out var name))
        {
            yield break;
        }

        if (!context.SoulInfoData.TryGetValue(id, out var info))
        {
            yield break;
        }

        var skillName = context.SkillNameData[info.SkillId.ToString()];
        var skillOption = GetSkillOption(info, context.SkillOptionData);

        var soul = new MalibSoul
        {
            Id = id,
            Name = name,
            Skill = skillName,
            ChargeFactor = skillOption.IncTableId,
            Magnificent = info.IsMagnificent
        };
        if (info.IsMagnificent)
        {
            soul.Options = GetSoulRandomOptions(skillOption);
        }
        else
        {
            soul.Option = GetSoulOption(skillOption);
        }

        yield return soul;
    }

    private static SkillOption GetSkillOption(SoulInfo info,
        IReadOnlyDictionary<int, IList<SkillOption>> skillOptionData)
    {
        var skillOptions = skillOptionData[info.SkillId];
        var index = info.IsMagnificent ? 0 : info.Index;
        return skillOptions[index];
    }

    private static GearOption GetSoulOption(SkillOption sKillOption)
    {
        return sKillOption.Options[0];
    }

    private static MalibSoul.RandomOptions GetSoulRandomOptions(SkillOption skillOption)
    {
        return new MalibSoul.RandomOptions
        {
            AttackPower = skillOption.Options[0],
            MagicPower = skillOption.Options[1],
            AllStat = skillOption.Options[2],
            MaxHp = skillOption.Options[3],
            CriticalRate = skillOption.Options[4],
            IgnoreMonsterArmor = skillOption.Options[5],
            BossDamage = skillOption.Options[6]
        };
    }
}