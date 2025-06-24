using WzJson.Common;

namespace WzJson.Repository;

public class SoulNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Item/Consume/0259.img/*")
{
}