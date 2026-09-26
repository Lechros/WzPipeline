using Wz;

namespace WzPipeline.Domains.Soul;

public class SkillOptionNode(IWzNode node)
{
    public int Id => int.Parse(node.Name);

    public TempOptionNode[] TempOption => node.Nodes["tempOption"].Nodes.Select(n => new TempOptionNode(n)).ToArray();

    public class TempOptionNode(IWzNode node)
    {
        public int Id => node.Nodes["id"].GetInt32();
        public int Prob => node.Nodes["prob"].GetInt32();
    }
}
