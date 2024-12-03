using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class SkillReader(SkillNodeRepository skillNodeRepository, GlobalFindNodeFunction findNode)
    : AbstractWzReader
{
    public const string SkillIconPath = "skill-icon";

    protected override IEnumerable<Wz_Node> GetNodes() => skillNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters() =>
    [
        new IconBitmapConverter(SkillIconPath, "icon", findNode)
    ];
}