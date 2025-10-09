using FluentAssertions;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Tests;

public class GlobTraverserTests
{
    private IWzProvider wzProvider;

    [OneTimeSetUp]
    public void SetUp()
    {
        wzProvider = TestUtils.CreateWzProvider();
    }

    [Test]
    public void GearTraverser_Count_Equals()
    {
        var path =
            "Character/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*.img";
        var traverser = GlobTraverser.Create(wzProvider, path, TestUtils.DefaultNode.Create);

        var count = traverser.GetNodeCount();
        count.Should().NotBe(0);
        
        var enumeratedCount = traverser.EnumerateNodes().Count();
        count.Should().Be(enumeratedCount);
    }
}