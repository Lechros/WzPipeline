using WzComparerR2.WzLib;

namespace WzPipeline.Domains.SetItem;

public class SetItemNode(Wz_Node node)
{
    public string Id => node.Text;
    public string Name => node.Nodes["setItemName"].GetValue<string>();

    public IEnumerable<int> ItemIds
    {
        get
        {
            foreach (var subNode in node.Nodes["ItemID"].Nodes)
                if (subNode.Nodes.Count == 0)
                    yield return subNode.GetValue<int>();
                else
                    foreach (var partNode in subNode.Nodes)
                        switch (partNode.Text)
                        {
                            case "representName":
                            case "typeName":
                            case "byGender":
                                break;
                            default:
                                yield return partNode.GetValue<int>();
                                break;
                        }
        }
    }

    public IEnumerable<EffectNode> Effects =>
        node.Nodes["Effect"].Nodes.Select(effectNode => new EffectNode(effectNode));

    public bool JokerPossible => (node.Nodes["jokerPossible"]?.GetValue<int>() ?? 0) != 0;
    public bool ZeroWeaponJokerPossible => (node.Nodes["zeroWeaponJokerPossible"]?.GetValue<int>() ?? 0) != 0;

    public class EffectNode(Wz_Node effectNode)
    {
        public int Index => int.Parse(effectNode.Text);

        public IEnumerable<(string Type, int value)> Properties => effectNode.Nodes
            .Where(n => n.Text != "Option")
            .Select(n => (n.Text, n.GetValue<int>()));

        public IEnumerable<(int OptionCode, int Level)> Options => effectNode.Nodes["Option"]?.Nodes
            .Select(n => (n.Nodes["option"].GetValue<int>(), n.Nodes["level"].GetValue<int>())) ?? [];
    }
}