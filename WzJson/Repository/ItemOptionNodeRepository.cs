using WzJson.Common;

namespace WzJson.Repository;

public class ItemOptionNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Item/ItemOption.img/*")
{
}