using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Exporter;

public class SingleResultProvider<T> : AbstractExporter<T>
{
    public T? Result { get; private set; }

    public override void Export(IEnumerable<T> models, string path)
    {
        Result = models.Single();
    }
}