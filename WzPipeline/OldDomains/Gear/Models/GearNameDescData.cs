using WzPipeline.OldDomains.String;

namespace WzPipeline.OldDomains.Gear.Models;

public interface IGearNameDescData : IReadOnlyDictionary<string, NameDesc>
{
}

public class GearNameDescData : Dictionary<string, NameDesc>, IGearNameDescData
{
}