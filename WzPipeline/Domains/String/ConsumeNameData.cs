namespace WzPipeline.Domains.String;

public interface IConsumeNameData : IReadOnlyDictionary<string, string>
{
}

public class ConsumeNameData : Dictionary<string, string>, IConsumeNameData
{
}