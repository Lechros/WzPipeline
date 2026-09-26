using Wz;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Soul;

namespace WzPipeline.Tests;

public class SoulParserTests
{
    private readonly SoulParser parser = new();
    private readonly SoulNode first = new(new TestNode("2591085"));
    private readonly SoulNode second = new(new TestNode("2591086"));
    private Dictionary<int, SoulInfo> info = null!;
    private Dictionary<int, SkillOption> skills = null!;
    private HashSet<int> amplify = null!;
    private SoulParseContext context = null!;

    [SetUp]
    public void SetUp()
    {
        var entry = new SoulInfo { SoulIds = [2591085, 2591086] };
        info = new Dictionary<int, SoulInfo> { [2591085] = entry, [2591086] = entry };
        skills = new Dictionary<int, SkillOption>
        {
            [86] = new()
            {
                SkillOptionId = 86,
                Options =
                [
                    new GearOption { AttackPower = 5 },
                    new GearOption { MagicPower = 5 },
                    new GearOption { AllStatsSetter = 10 },
                    new GearOption { MaxHp = 1000 },
                    new GearOption { CriticalRate = 5 },
                    new GearOption { IgnoreMonsterArmor = 3 },
                    new GearOption { BossDamage = 3 }
                ]
            },
            [87] = new() { SkillOptionId = 87, Options = [new GearOption { MagicPower = 7 }] }
        };
        amplify = [];
        context = new SoulParseContext
        {
            ConsumeNameData = new Dictionary<string, string> { ["2591085"] = "First soul", ["2591086"] = "Second soul" },
            SoulInfoData = info,
            SkillOptionData = skills,
            SoulAmplifyData = amplify
        };
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Parse_UsesAmplifyMembershipForBothIds(bool magnificent)
    {
        skills[86] = new SkillOption { SkillOptionId = 86, Options = [new GearOption { AttackPower = 5 }] };
        if (magnificent) amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(first, context).Single();

        soul.Id.Should().Be(2591085);
        soul.Magnificent.Should().Be(magnificent);
        if (magnificent)
        {
            soul.Option.Should().BeNull();
            soul.Options![1].MagicPower.Should().Be(7);
            soul.Options.Should().HaveCount(2);
        }
        else
        {
            soul.Options.Should().BeNull();
            soul.Option!.AttackPower.Should().Be(5);
        }
        parser.Parse(second, context).Should().BeEmpty();
    }

    [TestCase(2591085)]
    [TestCase(2591086)]
    public void Parse_RejectsMixedAmplifyMembership(int amplifiableId)
    {
        amplify.Add(amplifiableId);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>()
            .WithMessage("*SoulAmplify*2591085*2591086*");
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Parse_MergesOptionsRegardlessOfIdOrder(bool reversed)
    {
        var attack = new GearOption { AttackPower = 5 };
        var magic = new GearOption { MagicPower = 7 };
        skills[86] = new SkillOption { SkillOptionId = 86, Options = [reversed ? magic : attack] };
        skills[87] = new SkillOption { SkillOptionId = 87, Options = [reversed ? attack : magic] };
        amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(first, context).Single();

        soul.Magnificent.Should().BeTrue();
        soul.Option.Should().BeNull();
        soul.Options![0].AttackPower.Should().Be(5);
        soul.Options![1].MagicPower.Should().Be(7);
        soul.Options.Should().HaveCount(2);
        parser.Parse(second, context).Should().BeEmpty();
    }

    [Test]
    public void Parse_RejectsMissingMagicPowerOption()
    {
        info[2591085] = new SoulInfo { SoulIds = [2591085] };
        skills[86] = new SkillOption { SkillOptionId = 86, Options = [new GearOption { AttackPower = 5 }] };
        amplify.Add(2591085);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>().WithMessage("*MagicPower*2591085*");
    }

    [Test]
    public void Parse_AcceptsMagnificentSoulWithOneId()
    {
        info[2591085] = new SoulInfo { SoulIds = [2591085] };
        amplify.Add(2591085);

        var soul = parser.Parse(first, context).Single();

        soul.Magnificent.Should().BeTrue();
        soul.Options![6].BossDamage.Should().Be(3);
    }

    [TestCase(5, 0)]
    [TestCase(6, 0)]
    [TestCase(0, 3)]
    public void Parse_RejectsOverlappingOptions(int attackPower, int attackPowerRate)
    {
        skills[86] = new SkillOption { SkillOptionId = 86, Options = [new GearOption { AttackPower = 5 }] };
        skills[87] = new SkillOption
        {
            SkillOptionId = 87,
            Options = [new GearOption { AttackPower = attackPower, AttackPowerRate = attackPowerRate }]
        };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>()
            .WithMessage("Overlapping AttackPower options*2591085*2591086*");
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Parse_UsesFirstRandomOptions(bool reversed)
    {
        skills[87] = new SkillOption
        {
            SkillOptionId = 87,
            Options = [new GearOption { AttackPower = 9 }, new GearOption { MagicPower = 7 }]
        };
        var entry = new SoulInfo { SoulIds = reversed ? [2591086, 2591085] : [2591085, 2591086] };
        info[2591085] = entry;
        info[2591086] = entry;
        amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(reversed ? second : first, context).Single();

        soul.Id.Should().Be(entry.SoulIds[0]);
        soul.Options![0].AttackPower.Should().Be(reversed ? 9 : 5);
        soul.Options![1].MagicPower.Should().Be(reversed ? 7 : 5);
        if (reversed) soul.Options.Should().HaveCount(2);
        else soul.Options![6].BossDamage.Should().Be(3);
        parser.Parse(reversed ? first : second, context).Should().BeEmpty();
    }

    [Test]
    public void Parse_RejectsMissingMagicPowerInFirstRandomOptions()
    {
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options = [new GearOption { AttackPower = 5 }, new GearOption { BossDamage = 3 }]
        };
        skills[87] = new SkillOption
        {
            SkillOptionId = 87,
            Options = [new GearOption { MagicPower = 7 }, new GearOption { MaxHp = 1000 }]
        };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>().WithMessage("Missing MagicPower option*2591085*2591086*");
    }

    [Test]
    public void Parse_RejectsOverlappingOptionsInFirstRandomOptions()
    {
        info[2591085] = new SoulInfo { SoulIds = [2591085] };
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options =
            [
                new GearOption { AttackPower = 5 },
                new GearOption { AttackPowerRate = 3 },
                new GearOption { MagicPower = 7 }
            ]
        };
        amplify.Add(2591085);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>().WithMessage("Overlapping AttackPower options*2591085*");
    }

    [TestCase(10, 10, 10, 10, 0, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 5, 5, 5, 5)]
    public void Parse_AcceptsMatchingAllStats(
        int str, int dex, int intelligence, int luk,
        int strRate, int dexRate, int intRate, int lukRate)
    {
        var allStat = new GearOption
        {
            Str = str, Dex = dex, Int = intelligence, Luk = luk,
            StrRate = strRate, DexRate = dexRate, IntRate = intRate, LukRate = lukRate
        };
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options = [new GearOption { AttackPower = 5 }, new GearOption { MagicPower = 5 }, allStat]
        };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().NotThrow();
    }

    [TestCase(9, 10, 10, 10, 0, 0, 0, 0)]
    [TestCase(10, 9, 10, 10, 0, 0, 0, 0)]
    [TestCase(10, 10, 9, 10, 0, 0, 0, 0)]
    [TestCase(10, 10, 10, 9, 0, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 4, 5, 5, 5)]
    [TestCase(0, 0, 0, 0, 5, 4, 5, 5)]
    [TestCase(0, 0, 0, 0, 5, 5, 4, 5)]
    [TestCase(0, 0, 0, 0, 5, 5, 5, 4)]
    [TestCase(-1, -1, -1, -1, 0, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, -1, -1, -1, -1)]
    public void Parse_RejectsInvalidAllStats(
        int str, int dex, int intelligence, int luk,
        int strRate, int dexRate, int intRate, int lukRate)
    {
        var allStat = new GearOption
        {
            Str = str, Dex = dex, Int = intelligence, Luk = luk,
            StrRate = strRate, DexRate = dexRate, IntRate = intRate, LukRate = lukRate
        };
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options = [new GearOption { AttackPower = 5 }, new GearOption { MagicPower = 5 }, allStat]
        };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<InvalidDataException>().WithMessage("Invalid AllStat option.*");
    }

    [Test]
    public void Parse_RejectsMissingAttackPowerOption()
    {
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options = [new GearOption { MagicPower = 5 }, new GearOption { BossDamage = 3 }]
        };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>().WithMessage("Missing AttackPower option*");
    }

    [Test]
    public void Parse_OmitsOptionsWithoutRecognizedStats()
    {
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options =
            [
                new GearOption { AttackPower = 5 }, new GearOption { MagicPower = 5 },
                new GearOption(), new GearOption { AllStat = 5 }
            ]
        };
        amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(first, context).Single();

        soul.Options.Should().Equal(skills[86].Options.Take(2));
    }

    [Test]
    public void Parse_OrdersRandomOptionsByKind()
    {
        var expected = skills[86].Options;
        skills[86] = new SkillOption { SkillOptionId = 86, Options = expected.Reverse().ToArray() };
        amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(first, context).Single();

        soul.Options.Should().Equal(expected);
    }

    [Test]
    public void Parse_AcceptsPercentageOptions()
    {
        skills[86] = new SkillOption
        {
            SkillOptionId = 86,
            Options =
            [
                new GearOption { AttackPowerRate = 3 }, new GearOption { MagicPowerRate = 3 },
                new GearOption { AllStatRatesSetter = 5 }, new GearOption { MaxHpRate = 10 }
            ]
        };
        amplify.UnionWith([2591085, 2591086]);

        var soul = parser.Parse(first, context).Single();

        soul.Options.Should().Equal(skills[86].Options);
    }

    [TestCase(0, "AttackPower")]
    [TestCase(1, "MagicPower")]
    [TestCase(2, "AllStat")]
    [TestCase(3, "MaxHp")]
    [TestCase(4, "CriticalRate")]
    [TestCase(5, "IgnoreMonsterArmor")]
    [TestCase(6, "BossDamage")]
    public void Parse_RejectsDuplicateRandomOptionKinds(int index, string kind)
    {
        var options = skills[86].Options;
        skills[86] = new SkillOption { SkillOptionId = 86, Options = [.. options, options[index]] };
        amplify.UnionWith([2591085, 2591086]);

        var action = () => parser.Parse(first, context).ToArray();

        action.Should().Throw<DataFormatException>().WithMessage($"Overlapping {kind} options*");
    }

    private sealed class TestNode(string name) : IWzNode
    {
        public string Name => name;
        public WzNodeType Type => WzNodeType.Property;
        public IWzNode? Parent => null;
        public IWzNodeCollection<IWzNode> Nodes => throw new NotSupportedException();
    }
}
