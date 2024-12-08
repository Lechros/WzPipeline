using FluentAssertions;
using WzJson.Repository;

namespace WzJson.Tests;

public class SoulCollectionProviderTests(SoulCollectionProviderFixture soulCollectionProviderFixture)
    : IClassFixture<SoulCollectionProviderFixture>
{

    [Fact]
    public void SoulCollection_ContainsExpectedValues()
    {
        var soulCollection = soulCollectionProviderFixture.SoulCollectionProvider.SoulCollectionData;
        
        soulCollection.Items.Count.Should().BePositive();
        soulCollection.Items["0"].SoulSkill.Should().Be(80001266);
        soulCollection.Items["0"].SoulSkillH.Should().Be(80001270);
        soulCollection.Items["0"].SoulList["0"].Should().Be(2591075);
        soulCollection.Items["0"].SoulList["8"].Should().Be(2591088);
        
        soulCollection.Items["24"].SoulSkill.Should().Be(80001339);
        soulCollection.Items["24"].SoulSkillH.Should().BeNull();
    }
}

public class SoulCollectionProviderFixture : IDisposable
{
    public SoulCollectionProviderFixture()
    {
        var wzProviderFixture = new WzProviderFixture();
        var soulCollectionNodeRepository = new SoulCollectionNodeRepository(wzProviderFixture.WzProvider);
        SoulCollectionProvider = new SoulCollectionProvider(soulCollectionNodeRepository);
    }

    public SoulCollectionProvider SoulCollectionProvider { get; }

    public void Dispose()
    {
    }
}