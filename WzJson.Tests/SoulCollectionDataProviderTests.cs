using FluentAssertions;
using WzJson.Converter;
using WzJson.DataProvider;
using WzJson.Repository;

namespace WzJson.Tests;

[TestFixture]
public class SoulCollectionDataProviderTests
{
    private SoulCollectionDataProvider soulCollectionDataProvider;

    [OneTimeSetUp]
    public void SetUp()
    {
        var wzProvider = new WzProviderFixture().WzProvider;
        var soulCollectionNodeRepository = new SoulCollectionNodeRepository(wzProvider);
        soulCollectionDataProvider =
            new SoulCollectionDataProvider(soulCollectionNodeRepository, new SoulSkillInfoConverter());
    }

    [Test]
    public void SoulCollection_ContainsExpectedValues()
    {
        var soulCollection = soulCollectionDataProvider.Data;

        soulCollection.GetSoulSkillId(2591075).Should().Be(80001266);
        soulCollection.GetSoulSkillId(2591088).Should().Be(80001270);
        soulCollection.GetSoulSkillId(2591255).Should().Be(80001339);
    }
}