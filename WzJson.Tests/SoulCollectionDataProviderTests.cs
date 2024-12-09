using FluentAssertions;
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
        var wzProviderFixture = new WzProviderFixture();
        var soulCollectionNodeRepository = new SoulCollectionNodeRepository(wzProviderFixture.WzProvider);
        soulCollectionDataProvider = new SoulCollectionDataProvider(soulCollectionNodeRepository);
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