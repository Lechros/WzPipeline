using WzJson.Common;
using WzJson.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class SoulCollectionDataProvider(SoulCollectionNodeRepository soulCollectionNodeRepository, SoulSkillInfoConverter soulSkillInfoConverter)
    : AbstractDataProvider<SoulCollectionData>
{
    protected override SoulCollectionData GetData()
    {
        var processor = DefaultNodeProcessor.Of(soulSkillInfoConverter, () => new SoulCollectionData());
        return processor.ProcessNodes(soulCollectionNodeRepository.GetNodes());
    }
}