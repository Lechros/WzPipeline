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