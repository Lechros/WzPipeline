using WzJson.Domains.String;

namespace WzJson.Domains.Gear.Models;

public interface IGearNameDescData : IReadOnlyDictionary<string, NameDesc>
{
}

public class GearNameDescData : Dictionary<string, NameDesc>, IGearNameDescData
{
}