namespace WzJson.Domains.String;

public interface IEqpNameDescData : IReadOnlyDictionary<string, NameDesc>
{
}

public class EqpNameDescData : Dictionary<string, NameDesc>, IEqpNameDescData
{
}