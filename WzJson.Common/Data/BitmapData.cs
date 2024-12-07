using System.Drawing;

namespace WzJson.Common.Data;

public class BitmapData : ILabeledData, IDisposable
{
    public BitmapData(string label, string path, IDictionary<string, Bitmap>? items = null)
    {
        Label = label;
        Path = path;
        Items = items ?? new Dictionary<string, Bitmap>();
    }
    
    public string Label { get; set; }
    public string Path { get; }
    public IDictionary<string, Bitmap> Items { get; }

    public void Add<T>(string key, T item) where T : notnull
    {
        Items.Add(key, item as Bitmap);
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