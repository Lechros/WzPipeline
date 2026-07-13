namespace WzPipeline.Domains.Gear;

public enum GearTrade
{
    /** 교환 가능 */
    Tradeable = 0,
    /** 교환 불가 */
    TradeBlock = 1,
    /** 장착 시 교환 불가 */
    EquipTradeBlock = 2,
    /** 1회 교환 가능 */
    TradeOnce = 3
}

public enum GearShare
{
    /** 없음 */
    None = 0,
    /** 월드 내 나의 캐릭터 간 이동 가능 */
    AccountSharable = 1,
    /** 월드 내 나의 캐릭터 간 1회 이동 가능 (이동 후 교환불가) */
    AccountSharableOnce = 2
}

public enum GearCapability
{
    /** 설정 불가 */
    Cannot = 0,
    /** 설정 가능 */
    Can = 1,
    /** 재설정 불가 */
    Fixed = 2
}

public enum GearCuttable
{
    /** 없음 */
    None = 0,
    /** 카르마의 가위 또는 실버 카르마의 가위를 사용하면 1회 교환이 가능하게 할 수 있습니다. */
    Silver = 1,
    /** 플래티넘 카르마의 가위를 사용하면 1회 교환이 가능하게 할 수 있습니다. */
    Platinum = 2
}