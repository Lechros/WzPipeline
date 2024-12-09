using FluentAssertions;
using WzJson.DataProvider;
using WzJson.Repository;

namespace WzJson.Tests;

public class SoulCollectionDataProviderTests(SoulCollectionProviderFixture soulCollectionProviderFixture)
    : IClassFixture<SoulCollectionProviderFixture>
{

    [Fact]
    public void SoulCollection_ContainsExpectedValues()
    {
        var soulCollection = soulCollectionProviderFixture.SoulCollectionDataProvider.Data;
        
        soulCollection.GetSoulSkillId(2591075).Should().Be(80001266);
        soulCollection.GetSoulSkillId(2591088).Should().Be(80001270);
        soulCollection.GetSoulSkillId(2591255).Should().Be(80001339);
    }
}

public class SoulCollectionProviderFixture : IDisposable
{
    public SoulCollectionProviderFixture()
    {
        var wzProviderFixture = new WzProviderFixture();
        var soulCollectionNodeRepository = new SoulCollectionNodeRepository(wzProviderFixture.WzProvider);
        SoulCollectionDataProvider = new SoulCollectionDataProvider(soulCollectionNodeRepository);
    }

    public SoulCollectionDataProvider SoulCollectionDataProvider { get; }

    public void Dispose()
    {
    }
}