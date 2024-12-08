using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Domain;

namespace WzJson.Converter;

public class SoulSkillInfoConverter : AbstractNodeConverter<SoulSkillInfo>
{
    public SoulCollectionData Convert(IEnumerable<Wz_Node> nodes) => (SoulCollectionData)base.Convert(nodes);

    public override IData NewData() => new SoulCollectionData();

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override SoulSkillInfo? ConvertNode(Wz_Node node, string key)
    {
        var item = new SoulSkillInfo
        {
            SoulSkill = node.Nodes["soulSkill"].GetValue<int>(),
            SoulSkillH = node.Nodes["soulSkillH"]?.GetValue<int>()
        };
        foreach (var soulNode in node.Nodes["soulList"].Nodes)
        {
            item.SoulList.Add(soulNode.Text, soulNode.Nodes[0].GetValue<int>());
        }

        return item;
    }
}