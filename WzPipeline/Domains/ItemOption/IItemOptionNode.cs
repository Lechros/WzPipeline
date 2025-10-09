using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.ItemOption;

public interface IItemOptionNode : INode
{
    public int? OptionType { get; }
    public int? ReqLevel { get; }
    public string String { get; }
    public (int, Dictionary<string, string>)[] LevelOptions { get; }
}