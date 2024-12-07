namespace WzJson.Common;

public interface IWriter
{
    public bool Supports(IData data);
    public void Write(IData data, IProgress<WriteProgressData> progress);
}

public readonly record struct WriteProgressData(int Value, int MaxValue);