using WzJson.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class SoulCollectionDataProvider(SoulCollectionNodeRepository soulCollectionNodeRepository)
    : AbstractDataProvider<SoulCollectionData>
{
    protected override SoulCollectionData GetData()
    {
        return new SoulSkillInfoConverter().Convert(soulCollectionNodeRepository.GetNodes());
    }
}