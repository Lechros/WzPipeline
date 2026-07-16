using System.Drawing;
using System.Threading.Tasks.Dataflow;
using WzComparerR2;
using WzPipeline.Domains.Item;

namespace WzPipeline.Application.Item;

public class ItemIconExporter
{
    public IPropagatorBlock<ItemNode, KeyValuePair<string, Image>> CreateIconImageConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<ItemNode, KeyValuePair<string, Image>>(Convert);

        IEnumerable<KeyValuePair<string, Image>> Convert(ItemNode node)
        {
            var iconNode = node.GetIconNode(findNode);
            if (iconNode != null)
            {
                yield return KeyValuePair.Create<string, Image>(iconNode.Id, iconNode.Image);
            }
        }
    }

    public IPropagatorBlock<ItemNode, KeyValuePair<string, Point>> CreateIconOriginConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<ItemNode, KeyValuePair<string, Point>>(Convert);

        IEnumerable<KeyValuePair<string, Point>> Convert(ItemNode node)
        {
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

    public IPropagatorBlock<ItemNode, KeyValuePair<string, Image>> CreateIconRawImageConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<ItemNode, KeyValuePair<string, Image>>(Convert);

        IEnumerable<KeyValuePair<string, Image>> Convert(ItemNode node)
        {
            var iconNode = node.GetIconRawNode(findNode);
            if (iconNode != null)
            {
                yield return KeyValuePair.Create<string, Image>(iconNode.Id, iconNode.Image);
            }
        }
    }

    public IPropagatorBlock<ItemNode, KeyValuePair<string, Point>> CreateIconRawOriginConverterBlock(
        GlobalFindNodeFunction findNode)
    {
        return new TransformManyBlock<ItemNode, KeyValuePair<string, Point>>(Convert);

        IEnumerable<KeyValuePair<string, Point>> Convert(ItemNode node)
        {
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
}