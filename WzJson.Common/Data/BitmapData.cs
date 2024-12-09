using System.Drawing;

namespace WzJson.Common.Data;

public sealed class BitmapData(string label, string path)
    : AbstractKeyValueData<Bitmap>, ILabeled, IDisposable
{
    public string Label { get; } = label;
    public string Path { get; } = path;

    public void Dispose()
    {
        foreach (var bitmap in Values)
            bitmap.Dispose();

        GC.SuppressFinalize(this);
    }

    ~BitmapData()
    {
        Dispose();
    }
}