using WzJson.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class SoulSkillOptionDataProvider(
    SkillOptionNodeRepository skillOptionNodeRepository,
    SoulCollectionDataProvider soulCollectionDataProvider,
    ItemOptionDataProvider itemOptionDataProvider) : AbstractDataProvider<SkillOptionData>
{
    protected override SkillOptionData GetData()
    {
        return (SkillOptionData)new SkillOptionConverter(soulCollectionDataProvider.Data, itemOptionDataProvider.Data)
            .Convert(skillOptionNodeRepository.GetNodes());
    }
}