using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Domains.Gear;

public class GearStringBlockFactory(WzTree tree)
{
    public const string Pattern = "String/Eqp.img/Eqp/*/*";

    public ISourceBlock<StringNode> CreateSource()
    {
        return tree.MatchNodes(Pattern).ToSourceBlock().Map(node => new StringNode(node));
    }

    public TransformBlock<StringNode, KeyValuePair<string, GearStrings>> CreateConverter()
    {
        return new TransformBlock<StringNode, KeyValuePair<string, GearStrings>>(node =>
            KeyValuePair.Create(node.Key, new GearStrings { Name = node.Name, Desc = node.Desc }));
    }

    public ITargetBlock<KeyValuePair<string, GearStrings>> CreateDictionaryCollector(
        IDictionary<string, GearStrings> dictionary)
    {
        return new ActionBlock<KeyValuePair<string, GearStrings>>(kvp => dictionary.Add(kvp.Key, kvp.Value),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
    }
}