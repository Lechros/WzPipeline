using WzJson.Common;

namespace WzJson.Repository;

public class ItemNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Item/{Cash,Consume,Etc}/*.img/*")
{
}