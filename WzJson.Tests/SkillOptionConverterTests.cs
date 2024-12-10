using FluentAssertions;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Data;
using WzJson.DataProvider;
using WzJson.Repository;

namespace WzJson.Tests;

[TestFixture]
public class SkillOptionConverterTests
{
    private IWzProvider wzProvider;
    private SkillOptionNodeRepository skillOptionNodeRepository;
    private SkillOptionConverter skillOptionConverter;
    private DefaultNodeProcessor<SkillOptionNode, SkillOptionData> skillOptionProcessor;

    [OneTimeSetUp]
    public void SetUp()
    {
        wzProvider = new WzProviderFixture().WzProvider;
        skillOptionNodeRepository = new SkillOptionNodeRepository(wzProvider);
        var soulCollectionDataProvider = new SoulCollectionDataProvider(new SoulCollectionNodeRepository(wzProvider), new SoulSkillInfoConverter());
        var itemOptionDataProvider = new ItemOptionDataProvider(new ItemOptionNodeRepository(wzProvider), new ItemOptionConverter());
        skillOptionConverter = new SkillOptionConverter(soulCollectionDataProvider, itemOptionDataProvider);
        skillOptionProcessor = DefaultNodeProcessor.Of(skillOptionConverter, () => new SkillOptionData());
    }

    [Test]
    public void Convert_ShouldReturnNonEmptySkillOptionData()
    {
        var data = skillOptionProcessor.ProcessNodes(skillOptionNodeRepository.GetNodes());

        data.Should().BeOfType<SkillOptionData>();
        data.As<SkillOptionData>().Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void Convert_ShouldContainExpectedNumberOfNodesGroupBySkillId()
    {
        var data = skillOptionProcessor.ProcessNodes(skillOptionNodeRepository.GetNodes());
        
        data.As<SkillOptionData>().GetNodesBySkillId(80003753).Should().HaveCount(16);
    }
}