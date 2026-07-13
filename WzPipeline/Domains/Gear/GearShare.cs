namespace WzPipeline.Domains.Gear;

public enum GearShare
{
    /** 없음 */
    None = 0,
    /** 월드 내 나의 캐릭터 간 이동 가능 */
    AccountSharable = 1,
    /** 월드 내 나의 캐릭터 간 1회 이동 가능 (이동 후 교환불가) */
    AccountSharableOnce = 2
}