using System.Collections.Immutable;
using WzJson.Model;

namespace WzJson.Domain;

public static partial class SoulResource
{
    public static readonly ImmutableArray<string> KnownSoulNamePrefixes =
        ["기운찬", "날렵한", "총명한", "놀라운", "화려한", "강력한", "빛나는", "강인한", "풍부한", "위대한"];

    public static readonly ImmutableArray<string> KnownSoulNameSuffixes = ["의 소울", "소울"];

    public static class KnownSoulNames
    {
        public const string 림보 = "림보";
        public const string 섬멸병기_스우 = "섬멸병기 스우";
        public const string 카링 = "카링";
        public const string 칼로스 = "칼로스";
        public const string 듄켈 = "듄켈";
        public const string 진_힐라 = "진 힐라";
        public const string 윌 = "윌";
        public const string 루시드 = "루시드";
        public const string 데미안 = "데미안";
        public const string 스우 = "스우";
        public const string 시그너스 = "시그너스";
        public const string 매그너스 = "매그너스";
        public const string 무르무르 = "무르무르";
        public const string 블러디퀸 = "블러디퀸";
        public const string 벨룸 = "벨룸";
        public const string 핑크빈 = "핑크빈";
        public const string 피에르 = "피에르";
        public const string 반반 = "반반";
        public const string 우르스 = "우르스";
        public const string 아카이럼 = "아카이럼";
        public const string 모카딘 = "모카딘";
        public const string 카리아인 = "카리아인";
        public const string CQ57 = "CQ57";
        public const string 줄라이 = "줄라이";
        public const string 플레드 = "플레드";
        public const string 파풀라투스 = "파풀라투스";
        public const string 힐라 = "힐라";
        public const string 반_레온 = "반 레온";
        public const string 자쿰 = "자쿰";
        public const string 발록 = "발록";
        public const string 돼지바 = "돼지바";
        public const string 프리미엄PC방 = "프리미엄PC방";
        public const string 무공 = "무공";
        public const string 피아누스 = "피아누스";
        public const string 드래곤_라이더 = "드래곤 라이더";
        public const string 렉스 = "렉스";
        public const string 에피네아 = "에피네아";
        public const string 핑크몽 = "핑크몽";
        public const string 락_스피릿 = "락 스피릿";
        public const string 교도관_아니 = "교도관 아니";
        public const string 크세르크세스 = "크세르크세스";
        public const string 캡틴_블랙_슬라임 = "캡틴 블랙 슬라임";
    }

    public static readonly IReadOnlyDictionary<string, int> KnownNormalSkillIds = new Dictionary<string, int>
    {
        [KnownSoulNames.림보] = 80003756,
        [KnownSoulNames.카링] = 80003259,
        [KnownSoulNames.칼로스] = 80003147,
        [KnownSoulNames.듄켈] = 80002640,
        [KnownSoulNames.진_힐라] = 80002638,
        [KnownSoulNames.윌] = 80002405,
        [KnownSoulNames.루시드] = 80002230,
        [KnownSoulNames.데미안] = 80001984,
        [KnownSoulNames.스우] = 80001696,
        [KnownSoulNames.섬멸병기_스우] = 80003640,
        [KnownSoulNames.매그너스] = 80001340,
        [KnownSoulNames.시그너스] = 80001266,
        [KnownSoulNames.블러디퀸] = 80001699,
        [KnownSoulNames.벨룸] = 80001694,
        [KnownSoulNames.무르무르] = 80001395,
        [KnownSoulNames.핑크빈] = 80001806,
        [KnownSoulNames.반반] = 80001684,
        [KnownSoulNames.피에르] = 80001680,
        [KnownSoulNames.우르스] = 80001807,
        [KnownSoulNames.아카이럼] = 80001281,
        [KnownSoulNames.모카딘] = 80001493,
        [KnownSoulNames.카리아인] = 80001495,
        [KnownSoulNames.줄라이] = 80001499,
        [KnownSoulNames.CQ57] = 80001497,
        [KnownSoulNames.플레드] = 80001501,
        [KnownSoulNames.파풀라투스] = 80002333,
        [KnownSoulNames.힐라] = 80001322,
        [KnownSoulNames.반_레온] = 80001802,
        [KnownSoulNames.자쿰] = 80001803,
        [KnownSoulNames.발록] = 80001797,
        [KnownSoulNames.돼지바] = 80001772,
        [KnownSoulNames.프리미엄PC방] = 80001780,
        [KnownSoulNames.무공] = 80001805,
        [KnownSoulNames.피아누스] = 80001321,
        [KnownSoulNames.렉스] = 80001799,
        [KnownSoulNames.드래곤_라이더] = 80001801,
        [KnownSoulNames.에피네아] = 80001280,
        [KnownSoulNames.핑크몽] = 80001948,
        [KnownSoulNames.교도관_아니] = 80001800,
        [KnownSoulNames.락_스피릿] = 80001804,
        [KnownSoulNames.캡틴_블랙_슬라임] = 80001339,
        [KnownSoulNames.크세르크세스] = 80001273
    };

    public static readonly IReadOnlyDictionary<string, int> KnownMagnificentSkillIds = new Dictionary<string, int>
    {
        [KnownSoulNames.림보] = 80003753,
        [KnownSoulNames.카링] = 80003260,
        [KnownSoulNames.칼로스] = 80003148,
        [KnownSoulNames.듄켈] = 80002641,
        [KnownSoulNames.진_힐라] = 80002639,
        [KnownSoulNames.윌] = 80002406,
        [KnownSoulNames.루시드] = 80002231,
        [KnownSoulNames.데미안] = 80001985,
        [KnownSoulNames.스우] = 80001697,
        [KnownSoulNames.섬멸병기_스우] = 80003641,
        [KnownSoulNames.매그너스] = 80001341,
        [KnownSoulNames.시그너스] = 80001270,
        [KnownSoulNames.블러디퀸] = 80001700,
        [KnownSoulNames.벨룸] = 80001695,
        [KnownSoulNames.무르무르] = 80001396,
        [KnownSoulNames.핑크빈] = 80001269,
        [KnownSoulNames.반반] = 80001685,
        [KnownSoulNames.피에르] = 80001698,
        [KnownSoulNames.우르스] = 80001808,
        [KnownSoulNames.아카이럼] = 80001282,
        [KnownSoulNames.모카딘] = 80001494,
        [KnownSoulNames.카리아인] = 80001496,
        [KnownSoulNames.줄라이] = 80001500,
        [KnownSoulNames.CQ57] = 80001498,
        [KnownSoulNames.플레드] = 80001502,
        [KnownSoulNames.파풀라투스] = 80002334,
        [KnownSoulNames.힐라] = 80001323,
        [KnownSoulNames.반_레온] = 80001268,
        [KnownSoulNames.자쿰] = 80001274,
        [KnownSoulNames.발록] = 80001267,
        [KnownSoulNames.돼지바] = 80001773,
        [KnownSoulNames.프리미엄PC방] = 80001781
    };

    public static readonly IReadOnlyDictionary<string, SoulTier> KnownSoulTiers = new Dictionary<string, SoulTier>
    {
        [KnownSoulNames.림보] = SoulTier.Tier1,
        [KnownSoulNames.카링] = SoulTier.Tier1,
        [KnownSoulNames.칼로스] = SoulTier.Tier1,
        [KnownSoulNames.듄켈] = SoulTier.Tier1,
        [KnownSoulNames.진_힐라] = SoulTier.Tier1,
        [KnownSoulNames.윌] = SoulTier.Tier1,
        [KnownSoulNames.루시드] = SoulTier.Tier1,
        [KnownSoulNames.데미안] = SoulTier.Tier1,
        [KnownSoulNames.스우] = SoulTier.Tier1,
        [KnownSoulNames.섬멸병기_스우] = SoulTier.Tier1,
        [KnownSoulNames.매그너스] = SoulTier.Tier1,
        [KnownSoulNames.시그너스] = SoulTier.Tier1,
        [KnownSoulNames.블러디퀸] = SoulTier.Tier1,
        [KnownSoulNames.벨룸] = SoulTier.Tier1,
        [KnownSoulNames.무르무르] = SoulTier.Tier1,
        [KnownSoulNames.핑크빈] = SoulTier.Tier2,
        [KnownSoulNames.반반] = SoulTier.Tier2,
        [KnownSoulNames.피에르] = SoulTier.Tier2,
        [KnownSoulNames.우르스] = SoulTier.Tier2,
        [KnownSoulNames.아카이럼] = SoulTier.Tier3,
        [KnownSoulNames.모카딘] = SoulTier.Tier3_AttackPowerTier2,
        [KnownSoulNames.카리아인] = SoulTier.Tier3_MagicPowerTier2,
        [KnownSoulNames.줄라이] = SoulTier.Tier3_MaxHpTier2,
        [KnownSoulNames.CQ57] = SoulTier.Tier3_CriticalRateTier2,
        [KnownSoulNames.플레드] = SoulTier.Tier3_AllStatTier2,
        [KnownSoulNames.파풀라투스] = SoulTier.Tier3,
        [KnownSoulNames.힐라] = SoulTier.Tier3,
        [KnownSoulNames.반_레온] = SoulTier.Tier4,
        [KnownSoulNames.자쿰] = SoulTier.Tier5,
        [KnownSoulNames.발록] = SoulTier.Tier6,
        [KnownSoulNames.돼지바] = SoulTier.Tier6,
        [KnownSoulNames.프리미엄PC방] = SoulTier.Tier6,
        [KnownSoulNames.무공] = SoulTier.Normal,
        [KnownSoulNames.피아누스] = SoulTier.Normal,
        [KnownSoulNames.렉스] = SoulTier.Normal,
        [KnownSoulNames.드래곤_라이더] = SoulTier.Normal,
        [KnownSoulNames.에피네아] = SoulTier.Normal,
        [KnownSoulNames.핑크몽] = SoulTier.Normal,
        [KnownSoulNames.교도관_아니] = SoulTier.Normal,
        [KnownSoulNames.락_스피릿] = SoulTier.Normal,
        [KnownSoulNames.캡틴_블랙_슬라임] = SoulTier.Normal,
        [KnownSoulNames.크세르크세스] = SoulTier.Normal
    };

    public static readonly IReadOnlyDictionary<SoulTier, Soul.RandomOptions> SoulRandomOptions =
        new Dictionary<SoulTier, Soul.RandomOptions>
        {
            [SoulTier.Tier1] = new()
            {
                AttackPower = new GearOption { AttackPowerRate = 3 },
                MagicPower = new GearOption { MagicPowerRate = 3 },
                AllStat = new GearOption { _AllStatRatesSetter = 5 },
                MaxHp = new GearOption { MaxHp = 2000 },
                CriticalRate = new GearOption { CriticalRate = 12 },
                BossDamage = new GearOption { BossDamage = 7 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 7 },
            },
            [SoulTier.Tier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 10 },
                MagicPower = new GearOption { MagicPower = 10 },
                AllStat = new GearOption { _AllStatsSetter = 20 },
                MaxHp = new GearOption { MaxHp = 1500 },
                CriticalRate = new GearOption { CriticalRate = 10 },
                BossDamage = new GearOption { BossDamage = 5 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 5 },
            },
            [SoulTier.Tier3] = new()
            {
                AttackPower = new GearOption { AttackPower = 8 },
                MagicPower = new GearOption { MagicPower = 8 },
                AllStat = new GearOption { _AllStatsSetter = 17 },
                MaxHp = new GearOption { MaxHp = 1300 },
                CriticalRate = new GearOption { CriticalRate = 8 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier3_AttackPowerTier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 10 },
                MagicPower = new GearOption { MagicPower = 8 },
                AllStat = new GearOption { _AllStatsSetter = 17 },
                MaxHp = new GearOption { MaxHp = 1300 },
                CriticalRate = new GearOption { CriticalRate = 8 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier3_MagicPowerTier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 8 },
                MagicPower = new GearOption { MagicPower = 10 },
                AllStat = new GearOption { _AllStatsSetter = 17 },
                MaxHp = new GearOption { MaxHp = 1300 },
                CriticalRate = new GearOption { CriticalRate = 8 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier3_AllStatTier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 8 },
                MagicPower = new GearOption { MagicPower = 8 },
                AllStat = new GearOption { _AllStatsSetter = 20 },
                MaxHp = new GearOption { MaxHp = 1300 },
                CriticalRate = new GearOption { CriticalRate = 8 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier3_MaxHpTier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 8 },
                MagicPower = new GearOption { MagicPower = 8 },
                AllStat = new GearOption { _AllStatsSetter = 17 },
                MaxHp = new GearOption { MaxHp = 1500 },
                CriticalRate = new GearOption { CriticalRate = 8 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier3_CriticalRateTier2] = new()
            {
                AttackPower = new GearOption { AttackPower = 8 },
                MagicPower = new GearOption { MagicPower = 8 },
                AllStat = new GearOption { _AllStatsSetter = 17 },
                MaxHp = new GearOption { MaxHp = 1300 },
                CriticalRate = new GearOption { CriticalRate = 10 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier4] = new()
            {
                AttackPower = new GearOption { AttackPower = 7 },
                MagicPower = new GearOption { MagicPower = 7 },
                AllStat = new GearOption { _AllStatsSetter = 15 },
                MaxHp = new GearOption { MaxHp = 1200 },
                CriticalRate = new GearOption { CriticalRate = 7 },
                BossDamage = new GearOption { BossDamage = 4 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 4 },
            },
            [SoulTier.Tier5] = new()
            {
                AttackPower = new GearOption { AttackPower = 6 },
                MagicPower = new GearOption { MagicPower = 6 },
                AllStat = new GearOption { _AllStatsSetter = 12 },
                MaxHp = new GearOption { MaxHp = 1100 },
                CriticalRate = new GearOption { CriticalRate = 6 },
                BossDamage = new GearOption { BossDamage = 3 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 3 },
            },
            [SoulTier.Tier6] = new()
            {
                AttackPower = new GearOption { AttackPower = 5 },
                MagicPower = new GearOption { MagicPower = 5 },
                AllStat = new GearOption { _AllStatsSetter = 10 },
                MaxHp = new GearOption { MaxHp = 1000 },
                CriticalRate = new GearOption { CriticalRate = 5 },
                BossDamage = new GearOption { BossDamage = 3 },
                IgnoreMonsterArmor = new GearOption { BossDamage = 3 },
            }
        };
}