using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Skill;

public class SkillParser : AbstractWzParser
{
    public const string SkillIconPath = "skill-icon";

    private readonly SkillNodeRepository skillNodeRepository;
    private readonly GlobalFindNodeFunction findNodeFunction;

    public SkillParser(SkillNodeRepository skillNodeRepository, GlobalFindNodeFunction findNodeFunction)
    {
        this.skillNodeRepository = skillNodeRepository;
        this.findNodeFunction = findNodeFunction;
    }

    protected override IEnumerable<Wz_Node> GetNodes() => skillNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        return new List<INodeConverter<object>>
        {
            new IconBitmapConverter(SkillIconPath, "icon", findNodeFunction)
        };
    }
}