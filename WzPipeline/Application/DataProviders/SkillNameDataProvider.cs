using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Skill;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class SkillNameDataProvider(SkillNameDataBlockFactory factory) : AsyncDataProvider<Dictionary<string, string>>
{
    protected override async Task<Dictionary<string, string>> CreateAsync()
    {
        var data = new Dictionary<string, string>();

        var source = factory.CreateSource();
        var converter = factory.CreateConverter();
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(converter, new DataflowLinkOptions { PropagateCompletion = true });
        converter.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}