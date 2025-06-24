using WzJson.Common;

namespace WzJson.Repository;

public class SoulCollectionNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Etc/SoulCollection.img/*")
{
}