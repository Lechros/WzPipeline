using System.Diagnostics;
using FluentAssertions;
using Newtonsoft.Json;
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

    [OneTimeSetUp]
    public void SetUp()
    {
        wzProvider = new WzProviderFixture().WzProvider;
        var soulCollectionData = new SoulCollectionDataProvider(new SoulCollectionNodeRepository(wzProvider)).Data;
        var itemOptionData =
            new ItemOptionConverter("", "").Convert(new ItemOptionNodeRepository(wzProvider).GetNodes());
        skillOptionNodeRepository = new SkillOptionNodeRepository(wzProvider);
        skillOptionConverter = new SkillOptionConverter(soulCollectionData, itemOptionData);
    }

    [Test]
    public void Convert_ShouldReturnNonEmptySkillOptionData()
    {
        var data = skillOptionConverter.Convert(skillOptionNodeRepository.GetNodes());

        data.Should().BeOfType<SkillOptionData>();
        data.As<SkillOptionData>().Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void Convert_ShouldContainExpectedNumberOfNodesGroupBySkillId()
    {
        var data = skillOptionConverter.Convert(skillOptionNodeRepository.GetNodes());
        
        data.As<SkillOptionData>().GetNodesBySkillId(80003753).Should().HaveCount(16);
    }
}