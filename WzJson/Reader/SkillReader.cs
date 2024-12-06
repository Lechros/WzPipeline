using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
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

    protected override IList<INodeConverter<object>> GetConverters(SkillReadOptions options)
    {
        var converters = new List<INodeConverter<object>>();
        if (options.SkillIconPath != null)
            converters.Add(new IconBitmapConverter(options.SkillIconPath, "icon", findNode));
        return converters;
    }
}