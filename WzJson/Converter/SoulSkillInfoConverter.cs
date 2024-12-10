using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Data;

namespace WzJson.Converter;

public class SoulSkillInfoConverter : AbstractNodeConverter<SoulSkillNode>
{
    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override SoulSkillNode? Convert(Wz_Node node, string key)
    {
        var item = new SoulSkillNode
        {
            SoulSkill = node.Nodes["soulSkill"].GetValue<int>(),
            SoulSkillH = node.Nodes["soulSkillH"]?.GetValue<int>()
        };
        foreach (var soulNode in node.Nodes["soulList"].Nodes)
        {
            item.SoulList.Add(soulNode.Nodes[0].GetValue<int>());
        }

        return item;
    }
}