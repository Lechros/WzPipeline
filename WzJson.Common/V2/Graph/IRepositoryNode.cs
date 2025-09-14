namespace WzJson.Common.V2.Graph;

public interface IRepositoryNode : IGraphNode
{
    public IRepository Repository { get; }

    public void AddChild(IConverterNode node);
}