using FluentAssertions;
using WzJson.Converter;
using WzJson.Data;
using WzJson.DataProvider;
using WzJson.Repository;

namespace WzJson.Tests;

[TestFixture]
public class SkillOptionConverterTests
{
    [Test]
    public void Convert()
    {
        var wzProvider = new WzProviderFixture().WzProvider;
        var soulCollectionData = new SoulCollectionDataProvider(new SoulCollectionNodeRepository(wzProvider)).Data;
        var itemOptionData = new ItemOptionConverter("", "").Convert(new ItemOptionNodeRepository(wzProvider).GetNodes());
        var converter = new SkillOptionConverter(soulCollectionData, itemOptionData);

        var data = converter.Convert(new SkillOptionNodeRepository(wzProvider).GetNodes());

        data.Should().BeOfType<SkillOptionData>();
        data.As<SkillOptionData>().Should().HaveCountGreaterThan(0);
    }
}