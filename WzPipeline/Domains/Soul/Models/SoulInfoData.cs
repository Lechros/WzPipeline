namespace WzPipeline.Domains.Soul.Models;

public interface ISoulInfoData : IReadOnlyDictionary<int, SoulInfo>
{
}

public class SoulInfoData : Dictionary<int, SoulInfo>, ISoulInfoData
{
}