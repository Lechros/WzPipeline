using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Common.Data;
using WzJson.Repository;

namespace WzJson.Reader;

public class SkillReadOptions : IReadOptions
{
    public string? SkillIconPath { get; set; }
}

public class SkillReader(SkillNodeRepository skillNodeRepository, GlobalFindNodeFunction findNode)
    : AbstractWzReader<SkillReadOptions>
{
    protected override INodeRepository GetNodeRepository(SkillReadOptions _) => skillNodeRepository;

    protected override IList<INodeProcessor> GetProcessors(SkillReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.SkillIconPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconBitmapConverter("icon", findNode),
                () => new BitmapData("icon", options.SkillIconPath)));
        return processors;
    }
}