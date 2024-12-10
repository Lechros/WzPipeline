using WzJson.Common;
using WzJson.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class SkillOptionDataProvider(
    SkillOptionNodeRepository skillOptionNodeRepository,
    SkillOptionConverter skillOptionConverter) : AbstractDataProvider<SkillOptionData>
{
    protected override SkillOptionData GetData()
    {
        var processor = DefaultNodeProcessor.Of(skillOptionConverter, () => new SkillOptionData());
        return processor.ProcessNodes(skillOptionNodeRepository.GetNodes());
    }
}