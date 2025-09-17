using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph;

public interface IRepositoryNode : IGraphNode
{
    public IRepository Repository { get; }

    public void AddChild(IConverterNode node);
}