using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.ItemOption;

public interface IItemOptionNode : INode
{
    public int? OptionType { get; }
    public int? ReqLevel { get; }
    public string String { get; }
    public (int, Dictionary<string, string>)[] LevelOptions { get; }
}