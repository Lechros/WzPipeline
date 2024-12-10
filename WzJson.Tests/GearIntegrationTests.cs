using FluentAssertions;
using FluentAssertions.Execution;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.DataProvider;
using WzJson.Domain;
using WzJson.Model;
using WzJson.Reader;
using WzJson.Repository;

namespace WzJson.Tests;

public class GearIntegrationTests
{
    private Dictionary<string, Gear> gears;
    private Dictionary<string, WzComparerR2Gear> wzGears;

    [OneTimeSetUp]
    public void SetUp()
    {
        var wzProvider = new WzProviderFixture().WzProvider;
        var globalStringDataProvider = new GlobalStringDataProvider(
            new StringConsumeNodeRepository(wzProvider),
            new StringEqpNodeRepository(wzProvider),
            new StringSkillNodeRepository(wzProvider),
            new WzStringConverter());
        var itemOptionDataProvider =
            new ItemOptionDataProvider(
                new ItemOptionNodeRepository(wzProvider),
                new ItemOptionConverter());
        var reader = new GearReader(
            wzProvider.FindNode,
            new GearNodeRepository(wzProvider),
            new GearConverter(
                globalStringDataProvider,
                itemOptionDataProvider,
                wzProvider.FindNode));

        var options = new GearReadOptions
        {
            GearDataJsonPath = " "
        };
        gears = ((JsonData<Gear>)reader.Read(options, new Progress<ReadProgressData>())[0]).AsEnumerable()
            .ToDictionary();

        wzGears = new Dictionary<string, WzComparerR2Gear>();
        foreach (var node in new GearNodeRepository(wzProvider).GetNodes())
        {
            var wzGear = WzComparerR2Gear.CreateFromNode(node, wzProvider.FindNode);
            if (wzGear != null)
                wzGears.Add(wzGear.ItemID.ToString(), wzGear);
        }
    }


    [Test]
    public void SpecialGrade_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.specialGrade);
            var actual = gear.Attributes.SpecialGrade;
            actual.Should().Be(expected, Cuz(gear, "has specialGrade"));
        }
    }

    [Test]
    public void PotentialGrade_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.Grade;
            var actual = gear.PotentialGrade;
            actual.Should().HaveSameValueAs(expected, Cuz(gear, "has Grade"));
        }
    }

    [Test]
    public void Only_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.only);
            var actual = gear.Attributes.Only;
            actual.Should().Be(expected, Cuz(gear, "has only"));
        }
    }

    [Test]
    public void Trade_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var tradeBlock = wzGear.GetBooleanValue(GearPropType.tradeBlock);
            var equipTradeBlock = wzGear.GetBooleanValue(GearPropType.equipTradeBlock);
            if (tradeBlock)
            {
                gear.Attributes.Trade.Should().Be(GearAttribute.GearTrade.TradeBlock, Cuz(gear, "has tradeBlock"));
            }
            else if (equipTradeBlock)
            {
                gear.Attributes.Trade.Should()
                    .Be(GearAttribute.GearTrade.EquipTradeBlock, Cuz(gear, "has equipTradeBlock"));
            }
            else
            {
                gear.Attributes.Trade.Should().Be(GearAttribute.GearTrade.Tradeable, Cuz(gear, "has no prop"));
            }
        }
    }

    [Test]
    public void OnlyEquip_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.onlyEquip);
            var actual = gear.Attributes.OnlyEquip;
            actual.Should().Be(expected, Cuz(gear, "has onlyEquip"));
        }
    }

    [Test]
    public void AccountSharable_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var accountSharable = wzGear.GetBooleanValue(GearPropType.accountSharable);
            var sharableOnce = wzGear.GetBooleanValue(GearPropType.sharableOnce);
            if (accountSharable && sharableOnce)
            {
                gear.Attributes.Share.Should().Be(GearAttribute.GearShare.AccountSharableOnce,
                    Cuz(gear, "has accountSharable and sharableOnce"));
            }
            else if (accountSharable && !sharableOnce)
            {
                gear.Attributes.Share.Should()
                    .Be(GearAttribute.GearShare.AccountSharable, Cuz(gear, "has accountSharable"));
            }
            else
            {
                gear.Attributes.Share.Should().Be(GearAttribute.GearShare.None, Cuz(gear, "has no prop"));
            }
        }
    }

    [Test]
    public void BlockGoldHammer_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.blockGoldHammer);
            var actual = gear.Attributes.BlockGoldenHammer;
            actual.Should().Be(expected, Cuz(gear, "has blockGoldHammer"));
        }
    }

    [Test]
    public void NoPotential_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var noPotential = wzGear.GetBooleanValue(GearPropType.noPotential);
            var fixedPotential = wzGear.GetBooleanValue(GearPropType.fixedPotential);
            var tucIgnoreForPotential = wzGear.GetBooleanValue(GearPropType.tucIgnoreForPotential);
            if (noPotential)
            {
                gear.Attributes.CanPotential.Should()
                    .Be(GearAttribute.PotentialCan.Cannot, Cuz(gear, "has noPotential"));
            }
            else if (fixedPotential)
            {
                gear.Attributes.CanPotential.Should()
                    .Be(GearAttribute.PotentialCan.Fixed, Cuz(gear, "has fixedPotential"));
            }
            else if (tucIgnoreForPotential)
            {
                gear.Attributes.CanPotential.Should()
                    .Be(GearAttribute.PotentialCan.Can, Cuz(gear, "has tucIgnoreForPotential"));
            }
            else
            {
                gear.Attributes.CanPotential.Should().Be(GearAttribute.PotentialCan.None, Cuz(gear, "has no prop"));
            }
        }
    }

    [Test]
    public void Req_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            wzGear.Props.GetValueOrDefault(GearPropType.reqLevel)
                .Should().Be(gear.Req.Level, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqSTR)
                .Should().Be(gear.Req.Str, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqDEX)
                .Should().Be(gear.Req.Dex, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqINT)
                .Should().Be(gear.Req.Int, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqLUK)
                .Should().Be(gear.Req.Luk, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqJob)
                .Should().Be(gear.Req.Job, Cuz(gear));
            wzGear.Props.GetValueOrDefault(GearPropType.reqSpecJob)
                .Should().Be(gear.Req.Class, Cuz(gear));
        }
    }

    [Test]
    public void Superior_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.superiorEqp);
            var actual = gear.Attributes.Superior;
            actual.Should().Be(expected, Cuz(gear, "has superiorEqp"));
        }
    }

    [Test]
    public void ReduceReq_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.Props.GetValueOrDefault(GearPropType.reduceReq);
            var actual = gear.BaseOption.ReqLevelDecrease;
            actual.Should().Be(expected, Cuz(gear, "has reduceReq"));
        }
    }

    [Test]
    public void CannotUpgrade_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.exceptUpgrade);
            var actual = gear.Attributes.CannotUpgrade;
            actual.Should().Be(expected, Cuz(gear, "has exceptUpgrade"));
        }
    }

    [Test]
    public void ScrollUpgradeableCount_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.Props.GetValueOrDefault(GearPropType.tuc);
            var actual = gear.ScrollUpgradeableCount;
            actual.Should().Be(expected, Cuz(gear, "has tuc"));
        }
    }

    [Test]
    public void CanAdditionalPotential_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var fixedPotential = wzGear.GetBooleanValue(GearPropType.fixedPotential);
            if (fixedPotential)
            {
                gear.Attributes.CanAdditionalPotential.Should()
                    .Be(GearAttribute.PotentialCan.Cannot, Cuz(gear, "has fixedPotential"));
            }
            else
            {
                gear.Attributes.CanAdditionalPotential.Should()
                    .Be(GearAttribute.PotentialCan.None, Cuz(gear, "has no prop"));
            }
        }
    }

    [Test]
    public void CuttableCount_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            int? expected = wzGear.Props.TryGetValue(GearPropType.CuttableCount, out var value) ? value : null;
            var actual = gear.Attributes.CuttableCount;
            actual.Should().Be(expected, Cuz(gear, "has CuttableCount"));
        }
    }

    [Test]
    public void Potentials_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            if (wzGear.Options.Count(o => o != null) == 0)
            {
                gear.Potentials.Should().BeNull(Cuz(gear, "has 0 options"));
            }
            else
            {
                gear.Potentials.Should().NotBeNullOrEmpty(Cuz(gear));
                for (var i = 0; i < 3; i++)
                {
                    var potential = gear.Potentials![i];
                    var option = wzGear.Options[i];
                    if (option == null)
                    {
                        potential.Should().BeNull(Cuz(gear, "option is null at index " + i));
                    }
                    else
                    {
                        potential.Should().NotBeNull(Cuz(gear, "option is not null at index " + i));
                        potential!.Title.Should().Be(wzGear.Options[i].ConvertSummary(),
                            Cuz(gear, "option summary at index " + i));
                    }
                }
            }
        }
    }

    [Test]
    public void AdditionalPotentials_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            wzGear.AdditionalOptions.Should().AllSatisfy(o => o.Should().BeNull(),
                Cuz(gear, "having additional option is not implemented"));
        }
    }

    [Test]
    public void ExceptionalUpgradeableCount_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.Props.GetValueOrDefault(GearPropType.Etuc);
            var actual = gear.ExceptionalUpgradeableCount;
            actual.Should().Be(expected, Cuz(gear, "has Etuc"));
        }
    }

    [Test]
    public void Cuttable_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.Props.GetValueOrDefault(GearPropType.tradeAvailable);
            var actual = gear.Attributes.Cuttable;
            actual.Should().HaveValue(expected, Cuz(gear, "has tradeAvailable"));
        }
    }

    [Test]
    public void AccountShareTag_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.accountShareTag);
            var actual = gear.Attributes.AccountShareTag;
            actual.Should().Be(expected, Cuz(gear, "has accountShareTag"));
        }
    }

    [Test]
    public void Lucky_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            var expected = wzGear.GetBooleanValue(GearPropType.jokerToSetItem);
            var actual = gear.Attributes.Lucky;
            actual.Should().Be(expected, Cuz(gear, "has jokerToSetItem"));
        }
    }

    [Test]
    public void Incline_Equals()
    {
        using var scope = new AssertionScope();
        foreach (var (gear, wzGear) in GearPairs())
        {
            gear.Attributes.Incline.Charisma.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.charismaEXP), Cuz(gear));
            gear.Attributes.Incline.Insight.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.insightEXP), Cuz(gear));
            gear.Attributes.Incline.Will.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.willEXP), Cuz(gear));
            gear.Attributes.Incline.Craft.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.craftEXP), Cuz(gear));
            gear.Attributes.Incline.Sense.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.senseEXP), Cuz(gear));
            gear.Attributes.Incline.Charm.Should()
                .Be(wzGear.Props.GetValueOrDefault(GearPropType.charmEXP), Cuz(gear));
        }
    }

    private IEnumerable<(Gear, WzComparerR2Gear)> GearPairs()
    {
        foreach (var (key, gear) in gears)
        {
            yield return (gear, wzGears[key]);
        }
    }

    private static string Cuz(Gear gear, string message = null)
    {
        if (message == null)
            return $"gear(id={gear.Meta.Id})";
        return $"gear(id={gear.Meta.Id}) {message}";
    }
}