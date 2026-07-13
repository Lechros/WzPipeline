using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Domains.Skill;

public class SkillNameDataBlockFactory(WzTree tree) : Dictionary<string, string>
{
    public const string Pattern = "String/Skill.img/*";

    public ISourceBlock<StringNode> CreateSource()
    {
        return tree.MatchNodes(Pattern).ToSourceBlock().Map(node => new StringNode(node));
    }

    public TransformBlock<StringNode, KeyValuePair<string, string>> CreateConverter()
    {
        return new TransformBlock<StringNode, KeyValuePair<string, string>>(node =>
            KeyValuePair.Create(node.Key, node.Name)!);
    }

    public ITargetBlock<KeyValuePair<string, string>> CreateDictionaryCollector(IDictionary<string, string> dictionary)
    {
        return new ActionBlock<KeyValuePair<string, string>>(kvp => dictionary.Add(kvp.Key, kvp.Value),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
    }
}