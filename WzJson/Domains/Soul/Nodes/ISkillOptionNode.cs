using WzJson.Core.Stereotype;

namespace WzJson.Domains.Soul.Nodes;

public interface ISkillOptionNode : INode
{
    public int SkillId { get; }
    public int ReqLevel { get; }
    public int IncTableId { get; }
    public ITempOption[] TempOption { get; }

    public interface ITempOption
    {
        public int Id { get; }
    }
}