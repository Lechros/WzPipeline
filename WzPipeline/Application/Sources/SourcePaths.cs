namespace WzPipeline.Application.Sources;

public static class SourcePaths
{
    public const string Gear =
        "Character/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*.img";

    public const string Item = "Item/{Cash,Consume,Etc}/*.img/*";
    public const string SetItem = "Etc/SetItemInfo.img/*";
    public const string ExclusiveEquip = "Etc/ExclusiveEquip.img/*";
    public const string ConsumeName = "String/Consume.img/*";
    public const string Soul = "Item/Consume/0259.img/*";
    public const string SoulCollection = "Etc/SoulCollection.img/*";
    public const string SkillOption = "Item/SkillOption.img/skill/*";
    public const string ItemOption = "Item/ItemOption.img/*";
    public const string GearString = "String/Eqp.img/Eqp/*/*";
    public const string SkillName = "String/Skill.img/*";
    public const string AstraSubWeapon = "Etc/SubWeaponTransferData.img/Job/*";
}