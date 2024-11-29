using System.Text.RegularExpressions;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public partial class SoulConverter : INodeConverter<Model.Soul>
{
    public const string SoulDataJsonName = "soul-data.json";

    [GeneratedRegex(@"추가 잠재능력 : ([\w가-힣]+) \+(\d+)")]
    private static partial Regex SoulDescOptionRegex();

    private readonly NameDescData nameDescData;
    private readonly NameDescData soulSkillNameData;

    public SoulConverter(NameDescData nameDescData, NameDescData soulSkillNameData)
    {
        this.nameDescData = nameDescData;
        this.soulSkillNameData = soulSkillNameData;
    }

    public IData NewData() => new JsonData(SoulDataJsonName);

    public string GetNodeName(Wz_Node node) => WzUtility.GetNodeCode(node);

    public Model.Soul? ConvertNode(Wz_Node node, string name)
    {
        nameDescData.Items.TryGetValue(name, out var nameDesc);
        if (nameDesc?.Name == null) return null;
        if (!IsSoulName(nameDesc.Name)) return null;

        var magnificent = IsMagnificent(nameDesc.Name);
        var tradeBlock = IsTradeBlock(node);
        if (magnificent && tradeBlock || !magnificent && !tradeBlock) return null;

        var mobName = GetSoulMobName(nameDesc.Name);
        var soul = new Model.Soul
        {
            Name = nameDesc.Name,
            Skill = GetSoulSkillName(mobName, magnificent),
            ChargeFactor = GetSoulChargeFactor(mobName),
            Magnificent = magnificent
        };
        if (magnificent)
            soul.Options = GetSoulRandomOptions(mobName);
        else
            soul.Option = GetSoulOption(nameDesc.Desc!);

        return soul;
    }

    private bool IsSoulName(string name)
    {
        var prefix = SoulResource.KnownSoulNamePrefixes.FirstOrDefault(name.StartsWith);
        var suffix = SoulResource.KnownSoulNameSuffixes.FirstOrDefault(name.EndsWith);
        return prefix != null && suffix != null;
    }

    private bool IsMagnificent(string soulName)
    {
        return soulName.StartsWith("위대한");
    }

    private bool IsTradeBlock(Wz_Node soulNode)
    {
        var tradeBlockNode = soulNode.FindNodeByPath(@"info\tradeBlock");
        if (tradeBlockNode == null) return false;
        return tradeBlockNode.GetValue(0) != 0;
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

    private string GetSoulSkillName(string mobName, bool magnificent)
    {
        var skillIds = magnificent ? SoulResource.KnownMagnificentSkillIds : SoulResource.KnownNormalSkillIds;
        var skillId = skillIds[mobName].ToString();
        return soulSkillNameData.Items[skillId].Name ??
               throw new ApplicationException("Skill name not found for: " + mobName);
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

    private Model.Soul.RandomOptions GetSoulRandomOptions(string mobName)
    {
        var soulTier = SoulResource.KnownSoulTiers[mobName];
        return SoulResource.SoulRandomOptions[soulTier];
    }
}