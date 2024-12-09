using System.Text.RegularExpressions;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Data;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public partial class SoulConverter(
    string dataLabel,
    string dataPath,
    GlobalStringData globalStringData,
    SoulCollectionData soulCollectionData)
    : AbstractNodeConverter<Soul>
{
    [GeneratedRegex(@"추가 잠재능력 : ([\w가-힣]+) \+(\d+)")]
    private static partial Regex SoulDescOptionRegex();

    public override IKeyValueData NewData() => new JsonData<Soul>(dataLabel, dataPath);

    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override Soul? ConvertNode(Wz_Node node, string key)
    {
        if (!int.TryParse(key, out var soulId) || !soulCollectionData.ContainsSoul(soulId)) return null;
        var skillId = soulCollectionData.GetSoulSkillId(soulId);
        var magnificent = soulCollectionData.IsMagnificentSoul(soulId);
        globalStringData.Consume.TryGetValue(key, out var soulString);
        if (soulString?.Name == null) return null;

        var mobName = GetSoulMobName(soulString.Name);
        var soul = new Soul
        {
            Name = soulString.Name,
            Skill = GetSoulSkillName(skillId),
            ChargeFactor = GetSoulChargeFactor(mobName),
            Magnificent = magnificent
        };
        if (magnificent)
            soul.Options = GetSoulRandomOptions(mobName);
        else
            soul.Option = GetSoulOption(soulString.Desc!);

        return soul;
    }

    private string GetSoulMobName(string soulName)
    {
        var prefix = SoulResource.KnownSoulNamePrefixes.First(soulName.StartsWith);
        var suffix = SoulResource.KnownSoulNameSuffixes.First(soulName.EndsWith);
        var start = prefix.Length + 1;
        var end = soulName.Length - suffix.Length;
        if (soulName[end - 1] == ' ') end--;
        return soulName[start..end];
    }

    private string GetSoulSkillName(int skillId)
    {
        return globalStringData.Skill[skillId.ToString()].Name ??
               throw new ApplicationException("Skill name not found for: " + skillId);
    }

    private int GetSoulChargeFactor(string mobName)
    {
        var soulTier = SoulResource.KnownSoulTiers[mobName];
        return soulTier != SoulResource.SoulTier.Normal ? 2 : 1;
    }

    private GearOption GetSoulOption(string soulDesc)
    {
        var match = SoulDescOptionRegex().Match(soulDesc);
        if (!match.Success) throw new ApplicationException("Invalid soul desc: " + soulDesc);

        var propName = match.Groups[1].ValueSpan;
        var value = int.Parse(match.Groups[2].ValueSpan);
        return propName switch
        {
            "힘" => new GearOption { Str = value },
            "민첩" => new GearOption { Dex = value },
            "지능" => new GearOption { Int = value },
            "행운" => new GearOption { Luk = value },
            "올스탯" => new GearOption { _AllStatsSetter = value },
            "MHP" or "MaxHP" => new GearOption { MaxHp = value },
            "MaxMP" => new GearOption { MaxMp = value },
            "공격력" => new GearOption { AttackPower = value },
            "마력" => new GearOption { MagicPower = value },
            "보스 공격력" => new GearOption { BossDamage = value },
            "방어력 무시" => new GearOption { IgnoreMonsterArmor = value },
            _ => throw new ApplicationException("Invalid soul desc(unknown prop name): " + soulDesc)
        };
    }

    private Soul.RandomOptions GetSoulRandomOptions(string mobName)
    {
        var soulTier = SoulResource.KnownSoulTiers[mobName];
        return SoulResource.SoulRandomOptions[soulTier];
    }
}