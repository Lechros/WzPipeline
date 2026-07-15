using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Soul;

public class SkillOptionNode(Wz_Node node)
{
    public string Id => node.Text;
    public int SkillId => node.Nodes["skillId"].GetValue<int>();
    public int ReqLevel => node.Nodes["reqLevel"].GetValue<int>();
    public int IncTableId => node.Nodes["incTableID"]?.GetValue<int>() ?? 0;

    public TempOptionNode[] TempOption => node.Nodes["tempOption"].Nodes.Select(n => new TempOptionNode(n)).ToArray();

    public class TempOptionNode(Wz_Node node)
    {
        public int Id => node.Nodes["id"].GetValue<int>();
        public int Prob => node.Nodes["prob"].GetValue<int>();
    }
}