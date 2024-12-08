using WzJson.Converter;
using WzJson.Domain;
using WzJson.Repository;

namespace WzJson;

public class SoulCollectionProvider
{
    private readonly Lazy<SoulCollectionData> lazyData;

    public SoulCollectionProvider(SoulCollectionNodeRepository soulCollectionNodeRepository)
    {
        lazyData = new Lazy<SoulCollectionData>(() => ReadData(soulCollectionNodeRepository));
    }

    public SoulCollectionData SoulCollectionData => lazyData.Value;

    private SoulCollectionData ReadData(SoulCollectionNodeRepository soulCollectionNodeRepository)
    {
        return new SoulSkillInfoConverter().Convert(soulCollectionNodeRepository.GetNodes());
    }
}