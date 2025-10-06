using WzComparerR2.WzLib;

namespace WzJson.Domains.Soul.Nodes;

public class SkillOptionNodeAdapter(Wz_Node node) : ISkillOptionNode
{
    public static SkillOptionNodeAdapter Create(Wz_Node node)
    {
        return new SkillOptionNodeAdapter(node);
    }

    public string Id => node.Text;
    public int SkillId => node.Nodes["skillId"].GetValue<int>();
    public int ReqLevel => node.Nodes["reqLevel"].GetValue<int>();
    public int IncTableId => node.Nodes["incTableID"]?.GetValue<int>() ?? 0;

    public ISkillOptionNode.ITempOption[] TempOption =>
        node.Nodes["tempOption"].Nodes.Select(SubNodeToAdapter).ToArray();

    private static ISkillOptionNode.ITempOption SubNodeToAdapter(Wz_Node node)
    {
        return new TempOptionNodeAdapter(node);
    }

    private class TempOptionNodeAdapter(Wz_Node node) : ISkillOptionNode.ITempOption
    {
        public int Id => node.Nodes["id"].GetValue<int>();
        public int Prob => node.Nodes["prob"].GetValue<int>();
    }
}