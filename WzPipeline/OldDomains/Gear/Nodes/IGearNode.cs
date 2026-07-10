using WzPipeline.OldDomains.Icon;

namespace WzPipeline.OldDomains.Gear.Nodes;

public interface IGearNode : IIconNode, IRawIconNode
{
    public bool IsCash { get; }
    public (int OptionCode, int Level)[]? Options { get; }
    /// <summary>
    /// icon, iconRaw, option, addition을 제외한 장비의 속성. onlyUpgrade는 binary로 반환.
    /// </summary>
    public IEnumerable<(string Type, int Value)> Properties { get; }
    public IEnumerable<int> ReqSpecJobs { get; }
}