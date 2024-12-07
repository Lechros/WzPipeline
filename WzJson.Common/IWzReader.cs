namespace WzJson.Common;

public interface IWzReader
{
    public IList<IData> Read(IReadOptions options, IProgress<ReadProgressData> progress);
}

public readonly record struct ReadProgressData(int Value, int MaxValue);