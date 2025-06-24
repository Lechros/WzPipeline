using WzJson.Common;

namespace WzJson.Repository;

public class StringConsumeNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "String/Consume.img/*")
{
}