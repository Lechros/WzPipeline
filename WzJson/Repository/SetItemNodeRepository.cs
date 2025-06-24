using WzJson.Common;

namespace WzJson.Repository;

public class SetItemNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Etc/SetItemInfo.img/*")
{
}