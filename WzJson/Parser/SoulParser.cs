using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class SoulParser : AbstractWzParser
{
    private readonly SoulNodeRepository soulNodeRepository;
    private readonly SoulStringNodeRepository soulStringNodeRepository;
    private readonly StringSkillNodeRepository stringSkillNodeRepository;

    public SoulParser(SoulNodeRepository soulNodeRepository, SoulStringNodeRepository soulStringNodeRepository,
        StringSkillNodeRepository stringSkillNodeRepository)
    {
        this.soulNodeRepository = soulNodeRepository;
        this.soulStringNodeRepository = soulStringNodeRepository;
        this.stringSkillNodeRepository = stringSkillNodeRepository;
    }

    protected override IEnumerable<Wz_Node> GetNodes() => soulNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var nameDescData = new NameDescConverter().Convert(soulStringNodeRepository.GetNodes());
        var soulSkillNameData = new NameDescConverter().Convert(stringSkillNodeRepository.GetNodes());
        return new List<INodeConverter<object>>
        {
            new Converter.SoulConverter(nameDescData, soulSkillNameData)
        };
    }
}