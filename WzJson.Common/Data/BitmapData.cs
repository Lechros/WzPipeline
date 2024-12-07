using System.Drawing;

namespace WzJson.Common.Data;

public sealed class BitmapData(string label, string path)
    : AbstractDictionaryData<Bitmap>(new Dictionary<string, Bitmap>()), ILabeled, IDisposable
{
    public string Label { get; } = label;
    public string Path { get; } = path;

    public void Dispose()
    {
        foreach (var bitmap in Items.Values)
            bitmap.Dispose();

        GC.SuppressFinalize(this);
    }

    ~BitmapData()
    {
        Dispose();
    }
}