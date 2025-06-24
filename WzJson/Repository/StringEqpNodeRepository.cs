using WzJson.Common;

namespace WzJson.Repository;

public class StringEqpNodeRepository(IWzProvider wzProvider) : GlobNodeRepositoryAdapter(wzProvider,
    "String/Eqp.img/Eqp/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*")
{
}