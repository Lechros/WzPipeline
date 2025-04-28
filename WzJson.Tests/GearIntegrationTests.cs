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
                gear.Potentials.Should().HaveSameCount(wzGear.Options.Where(o => o != null));
                for (var i = 0; i < gear.Potentials!.Length; i++)
                {
                    var potential = gear.Potentials![i];
                    var option = wzGear.Options[i];
                    potential.Should().NotBeNull(Cuz(gear, "option is not null at index " + i));
                    potential!.Summary.Should().Be(option.ConvertSummary(),
                        Cuz(gear, "option summary at index " + i));
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
            return $"gear(id={gear.Id})";
        return $"gear(id={gear.Id}) {message}";
    }
}