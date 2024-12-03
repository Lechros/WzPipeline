namespace WzJson.Common;

public interface IWriter
{
    public bool Supports(IData data);
    public void Write(IData data);
}