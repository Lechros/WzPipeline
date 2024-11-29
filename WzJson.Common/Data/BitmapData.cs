using System.Drawing;

namespace WzJson.Common.Data;

public class BitmapData : IData, IDisposable
{
    public BitmapData(string path, IDictionary<string, Bitmap>? items = null)
    {
        Path = path;
        Items = items ?? new Dictionary<string, Bitmap>();
    }

    public string Path { get; }
    public IDictionary<string, Bitmap> Items { get; }

    public void Add<T>(string name, T item) where T : notnull
    {
        Items.Add(name, item as Bitmap);
    }

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