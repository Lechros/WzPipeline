using WzJson.Common;

namespace WzJson.Repository;

public class GearNodeRepository(IWzProvider wzProvider) : GlobNodeRepositoryAdapter(wzProvider,
    "Character/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*.img")
{
}