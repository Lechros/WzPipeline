using System.Drawing;
using System.Threading.Tasks.Dataflow;
using WzComparerR2;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.Gear;

namespace WzPipeline.Application.Gear;

public class GearIconExporter
{
    public IPropagatorBlock<GearNode, KeyValuePair<string, Image>> CreateIconImageConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<GearNode, KeyValuePair<string, Image>>(Convert);

        IEnumerable<KeyValuePair<string, Image>> Convert(GearNode node)
        {
            if (node.Id == null)
            {
                yield break;
            }
            var iconNode = node.GetIconNode(findNode);
            if (iconNode != null)
            {
                yield return KeyValuePair.Create<string, Image>(iconNode.Id, iconNode.Image);
            }
        }
    }

    public IPropagatorBlock<GearNode, KeyValuePair<string, Point>> CreateIconOriginConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<GearNode, KeyValuePair<string, Point>>(Convert);

        IEnumerable<KeyValuePair<string, Point>> Convert(GearNode node)
        {
            if (node.Id == null)
            {
                yield break;
            }
            var iconNode = node.GetIconNode(findNode);
            if (iconNode != null)
            {
                var origin = iconNode.Origin;
                if (origin.HasValue)
                {
                    yield return KeyValuePair.Create(iconNode.Id, origin.Value);
                }
            }
        }
    }

    public IPropagatorBlock<GearNode, KeyValuePair<string, Image>> CreateIconRawImageConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<GearNode, KeyValuePair<string, Image>>(Convert);

        IEnumerable<KeyValuePair<string, Image>> Convert(GearNode node)
        {
            if (node.Id == null)
            {
                yield break;
            }
            var iconNode = node.GetIconRawNode(findNode);
            if (iconNode != null)
            {
                yield return KeyValuePair.Create<string, Image>(iconNode.Id, iconNode.Image);
            }
        }
    }

    public IPropagatorBlock<GearNode, KeyValuePair<string, Point>> CreateIconRawOriginConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<GearNode, KeyValuePair<string, Point>>(Convert);

        IEnumerable<KeyValuePair<string, Point>> Convert(GearNode node)
        {
            if (node.Id == null)
            {
                yield break;
            }
            var iconNode = node.GetIconRawNode(findNode);
            if (iconNode != null)
            {
                var origin = iconNode.Origin;
                if (origin.HasValue)
                {
                    yield return KeyValuePair.Create(iconNode.Id, origin.Value);
                }
            }
        }
    }

    public DataflowCollector<KeyValuePair<string, Point>, SortedDictionary<string, Point>>
        CreateOriginDictionaryCollector()
    {
        var data = new SortedDictionary<string, Point>();

        var target = new ActionBlock<KeyValuePair<string, Point>>(kvp => data.Add(kvp.Key, kvp.Value));

        return new DataflowCollector<KeyValuePair<string, Point>, SortedDictionary<string, Point>>
        {
            Target = target,
            Completion = target.Completion,
            Result = data
        };
    }
}