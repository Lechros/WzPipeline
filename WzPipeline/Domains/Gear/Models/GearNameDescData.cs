using WzPipeline.Domains.String;

namespace WzPipeline.Domains.Gear.Models;

public interface IGearNameDescData : IReadOnlyDictionary<string, NameDesc>
{
}

public class GearNameDescData : Dictionary<string, NameDesc>, IGearNameDescData
{
}