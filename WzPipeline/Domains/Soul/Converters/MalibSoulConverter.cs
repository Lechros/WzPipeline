using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;
using WzPipeline.Domains.Soul.Models;
using WzPipeline.Domains.Soul.Nodes;
using WzPipeline.Domains.String;

namespace WzPipeline.Domains.Soul.Converters;

public class MalibSoulConverter(
    IConsumeNameData soulNameData,
    ISkillNameData skillNameData,
    ISoulInfoData soulInfoData,
    ISkillOptionData skillOptionData) : AbstractConverter<ISoulNode, MalibSoul>
{
    public override MalibSoul? Convert(ISoulNode node)
    {
        if (!int.TryParse(node.Id, out var soulId))
        {
            return null;
        }

        if (!soulNameData.TryGetValue(node.Id, out var name))
        {
            return null;
        }

        if (!soulInfoData.TryGetValue(soulId, out var info))
        {
            return null;
        }

        var skillName = skillNameData[info.SkillId.ToString()];
        var skillOption = GetSkillOption(info);

        var soul = new MalibSoul
        {
            Id = soulId,
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

        return soul;
    }

    private SkillOption GetSkillOption(SoulInfo info)
    {
        var skillOptions = skillOptionData[info.SkillId];
        var index = info.IsMagnificent ? 0 : info.Index;
        return skillOptions[index];
    }

    private static GearOption GetSoulOption(SkillOption skillOption)
    {
        return skillOption.Options[0];
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