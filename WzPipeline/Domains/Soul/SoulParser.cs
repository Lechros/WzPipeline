using WzPipeline.Domains.Shared;

namespace WzPipeline.Domains.Soul;

public class SoulParser
{
    public IEnumerable<MalibRawSoul> Parse(SoulNode node, SoulParseContext context)
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

        // SoulInfo는 동일한 종류의 소울 목록이므로 첫 아이템만 출력
        if (id != info.SoulIds[0])
        {
            yield break;
        }

        yield return ParseSoul(id, name, info, context);
    }

    private static MalibRawSoul ParseSoul(int soulId, string name, SoulInfo info, SoulParseContext context)
    {
        var magnificent = context.SoulAmplifyData.Contains(soulId);
        if (info.SoulIds.Any(id => context.SoulAmplifyData.Contains(id) != magnificent))
            throw new DataFormatException(
                $"Inconsistent SoulAmplify membership within soulListEntry: {string.Join(", ", info.SoulIds)}");

        GearOption? option = null;
        GearOption[]? options = null;

        if (!magnificent)
        {
            option = GetSkillOption(info.SoulIds[0], context.SkillOptionData).Options[0];
        }
        else
        {
            options = GetSoulRandomOptions(info, context.SkillOptionData);
        }

        return new MalibRawSoul
        {
            Id = soulId,
            Name = name,
            Magnificent = magnificent,
            Option = option,
            Options = options,
        };
    }

    private static SkillOption GetSkillOption(int soulId, IReadOnlyDictionary<int, SkillOption> skillOptionData)
    {
        return skillOptionData[SoulInfo.GetSkillOptionId(soulId)];
    }

    private static GearOption[] GetSoulRandomOptions(SoulInfo soulInfo,
        IReadOnlyDictionary<int, SkillOption> skillOptionData)
    {
        var firstOptions = GetSkillOption(soulInfo.SoulIds[0], skillOptionData).Options;
        var gearOptions = firstOptions.Length > 1
            ? firstOptions
            : soulInfo.SoulIds.SelectMany(id => GetSkillOption(id, skillOptionData).Options).ToArray();

        var attackPower = FindOption(nameof(GearOption.AttackPower),
            o => o.AttackPower != 0 || o.AttackPowerRate != 0, required: true)!;
        var magicPower = FindOption(nameof(GearOption.MagicPower),
            o => o.MagicPower != 0 || o.MagicPowerRate != 0, required: true)!;
        var allStat = FindOption(nameof(GearOption.AllStat),
            o => o.Str != 0 || o.Dex != 0 || o.Int != 0 || o.Luk != 0 ||
                 o.StrRate != 0 || o.DexRate != 0 || o.IntRate != 0 || o.LukRate != 0);
        var maxHp = FindOption(nameof(GearOption.MaxHp), o => o.MaxHp != 0 || o.MaxHpRate != 0);
        var criticalRate = FindOption(nameof(GearOption.CriticalRate), o => o.CriticalRate != 0);
        var ignoreMonsterArmor = FindOption(nameof(GearOption.IgnoreMonsterArmor), o => o.IgnoreMonsterArmor != 0);
        var bossDamage = FindOption(nameof(GearOption.BossDamage), o => o.BossDamage != 0);

        if (allStat is not null &&
            !(allStat.Str > 0 && allStat.Str == allStat.Dex &&
              allStat.Str == allStat.Int && allStat.Str == allStat.Luk ||
              allStat.StrRate > 0 && allStat.StrRate == allStat.DexRate &&
              allStat.StrRate == allStat.IntRate && allStat.StrRate == allStat.LukRate))
            throw new InvalidDataException($"Invalid AllStat option. Option={allStat}");

        // FindOption already checks the nonzero values required by the other option types.
        GearOption?[] options = [attackPower, magicPower, allStat, maxHp, criticalRate, ignoreMonsterArmor, bossDamage];
        return options.OfType<GearOption>().ToArray();

        GearOption? FindOption(string name, Func<GearOption, bool> predicate, bool required = false)
        {
            var matches = gearOptions.Where(predicate).ToArray();
            if (matches.Length > 1)
                throw new DataFormatException(
                    $"Overlapping {name} options within soulListEntry: {string.Join(", ", soulInfo.SoulIds)}");
            if (required && matches.Length == 0)
                throw new DataFormatException(
                    $"Missing {name} option within soulListEntry: {string.Join(", ", soulInfo.SoulIds)}");
            return matches.SingleOrDefault();
        }
    }
}