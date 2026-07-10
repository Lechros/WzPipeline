using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Domains.SetItem;

public class SetItemDataBlockFactory(WzTree tree)
{
    public const string Pattern = "Etc/SetItemInfo.img/*";

    public ISourceBlock<SetItemNode> CreateSource()
    {
        return tree.MatchNodes(Pattern).ToSourceBlock().Map(node => new SetItemNode(node));
    }

    public TransformBlock<SetItemNode, MalibSetItem> CreateParser(ItemOptionData itemOptionData)
    {
        return new TransformBlock<SetItemNode, MalibSetItem>(node => new MalibSetItem
        {
            Id = int.Parse(node.Id),
            Name = node.Name,
            ItemIds = node.ItemIds.ToArray(),
            Effects = ConvertEffects(node.Effects, itemOptionData),
            JokerPossible = node.JokerPossible,
            ZeroWeaponJokerPossible = node.ZeroWeaponJokerPossible
        });
    }

    public ITargetBlock<MalibSetItem> CreateDictionaryCollector(IDictionary<int, MalibSetItem> dictionary)
    {
        return new ActionBlock<MalibSetItem>(entry => { dictionary.Add(entry.Id, entry); });
    }

    private static SortedDictionary<int, GearOption> ConvertEffects(IEnumerable<SetItemNode.EffectNode> effectNodes,
        ItemOptionData itemOptionData)
    {
        var result = new SortedDictionary<int, GearOption>();
        foreach (var effectNode in effectNodes)
        {
            var option = new GearOption();
            foreach (var (optionCode, level) in effectNode.Options)
                option.Add(itemOptionData[optionCode].Level[level].Option);

            foreach (var (type, value) in effectNode.Properties)
            {
                if (!Enum.TryParse(type, out GearPropType prop))
                    throw new NotImplementedException("Unknown set item property type: " + type);

                option.Add(prop, value);
            }

            result.Add(effectNode.Index, option);
        }

        return result;
    }
}