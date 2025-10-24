import { GearData } from "@malib/gear";

export type InputGear = GearData & {
  rawAttributes?: InputRawAttributes;
};

export interface InputRawAttributes {
  attackSpeed?: number;
  setItemID?: number;
  only?: number;
  tradeBlock?: number;
  accountSharable?: number;
  onlyEquip?: number;
  tradeAvailable?: number;
  equipTradeBlock?: number;
  sharableOnce?: number;
  notExtend?: number;
  charismaEXP?: number;
  senseEXP?: number;
  insightEXP?: number;
  willEXP?: number;
  craftEXP?: number;
  charmEXP?: number;
  accountShareTag?: number;
  noPotential?: number;
  fixedPotential?: number;
  specialGrade?: number;
  superiorEqp?: number;
  jokerToSetItem?: number;
  exceptUpgrade?: number;
  onlyUpgrade?: number;
  noLookChange?: number;
  tucIgnoreForPotential?: number;
  CuttableCount?: number;
  exUpgradeBlock?: number;
  exUpgradeChangeBlock?: number;
  bossReward?: number;
  setExtraOption?: number;
}
