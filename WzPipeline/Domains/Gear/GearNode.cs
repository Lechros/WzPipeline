using System.Collections;
using WzComparerR2;
using WzComparerR2.WzLib;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.Icon;

namespace WzPipeline.Domains.Gear;

public class GearNode(Wz_Node node)
{
    private Wz_Node InfoNode => node.Nodes["info"] ?? throw DataFormatException.MissingRequiredNode(node, "info");
    public int? Id => SafeIdParse(node.Text);

    public bool IsCash
    {
        get
        {
            var cashNode = InfoNode.Nodes["cash"];
            return cashNode != null && cashNode.GetValue<int>() != 0;
        }
    }

    public (int OptionCode, int Level)[]? Options
    {
        get
        {
            var optionNode = InfoNode.Nodes["option"];
            return optionNode?.Nodes
                .Select(n => (
                    n.Nodes["option"].GetValue<int>(),
                    n.Nodes["level"].GetValue<int>()))
                .ToArray();
        }
    }

    public IReadOnlyDictionary<GearPropType, int> Properties => new PropertyDictionary(InfoNode,
        new HashSet<string> { "icon", "iconRaw", "addition", "option" }, "onlyUpgrade");

    // TODO: separate onlyUpgrade into property

    public IEnumerable<int> ReqSpecJobs
    {
        get
        {
            var reqSpecJobsNode = InfoNode.FindNodeByPath("reqSpecJobs");
            if (reqSpecJobsNode != null)
            {
                foreach (var jobNode in reqSpecJobsNode.Nodes)
                {
                    yield return jobNode.GetValue<int>();
                }
            }
        }
    }

    public IconNode? GetIconNode(GlobalFindNodeFunction findNode)
    {
        var iconNode = InfoNode.FindNodeByPath("icon");
        return iconNode is null ? null : IconNode.Create(Id?.ToString() ?? "(null)", iconNode, findNode);
    }

    public IconNode? GetIconRawNode(GlobalFindNodeFunction findNode)
    {
        var iconRawNode = InfoNode.FindNodeByPath("iconRaw");
        return iconRawNode is null ? null : IconNode.Create(Id?.ToString() ?? "(null)", iconRawNode, findNode);
    }

    private static int? SafeIdParse(string text)
    {
        if (int.TryParse(text.Split('.')[0], out var id))
        {
            return id;
        }

        return null;
    }

    private class PropertyDictionary(Wz_Node infoNode, IReadOnlySet<string> ignoredProperties, string onlyUpgrade)
        : IReadOnlyDictionary<GearPropType, int>
    {
        private IEnumerable<(GearPropType Key, Wz_Node value)> Entries =>
            infoNode.Nodes
                .Where(node => !ignoredProperties.Contains(node.Text))
                .Select(node => (Enum.Parse<GearPropType>(node.Text), node));

        private int GetValue(Wz_Node node) => node.Text == onlyUpgrade ? node.Nodes.Count : node.GetValue<int>();

        public IEnumerator<KeyValuePair<GearPropType, int>> GetEnumerator() =>
            Entries.Select(e => KeyValuePair.Create(e.Key, GetValue(e.value))).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => infoNode.Nodes.Count(node => !ignoredProperties.Contains(node.Text));

        public bool ContainsKey(GearPropType key) => TryGetValue(key, out _);

        public bool TryGetValue(GearPropType key, out int value)
        {
            var node = infoNode.Nodes[key.ToString()];
            if (node is null || ignoredProperties.Contains(key.ToString()))
            {
                value = 0;
                return false;
            }

            value = GetValue(node);
            return true;
        }

        public int this[GearPropType key] => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();

        public IEnumerable<GearPropType> Keys => Entries.Select(e => e.Key);

        public IEnumerable<int> Values => Entries.Select(e => GetValue(e.value));
    }
}