import { GearAttributeData, GearData, GearShare, GearTrade } from "@malib/gear";
import { getCanAddOption } from "./addOption";
import { getCanAdditionalPotential, getCanPotential } from "./potential";
import { InputGear } from "./schema";
import { getCanStarforce } from "./starforce";
import { getCanScroll } from "./upgrade";
import { getGender } from "./gender";

export function process(input: InputGear): GearData {
  const { rawAttributes, ...rest } = input;

  const output: GearData = rest;

  output.req.gender = getGender(input);

  output.attributes = convertAttributes(input);

  const canStarforce = getCanStarforce(input);
  if (canStarforce) {
    output.attributes.canStarforce = canStarforce;
  }
  const canScroll = getCanScroll(input);
  if (canScroll) {
    output.attributes.canScroll = canScroll;
  }
  const canAddOption = getCanAddOption(input);
  if (canAddOption) {
    output.attributes.canAddOption = canAddOption;
  }
  const canPotential = getCanPotential(input);
  if (canPotential) {
    output.attributes.canPotential = canPotential;
  }
  const canAdditionalPotential = getCanAdditionalPotential(input);
  if (canAdditionalPotential) {
    output.attributes.canAdditionalPotential = canAdditionalPotential;
  }

  return output;
}

function convertAttributes(input: InputGear): GearAttributeData {
  const attributes: GearAttributeData = {};
  const raw = input.rawAttributes ?? {};

  if (raw.only) {
    attributes.only = true;
  }
  if (raw.tradeBlock) {
    attributes.trade = GearTrade.TradeBlock;
  } else if (raw.equipTradeBlock) {
    attributes.trade = GearTrade.EquipTradeBlock;
  }
  if (raw.onlyEquip) {
    attributes.onlyEquip = true;
  }
  if (raw.accountSharable) {
    if (raw.sharableOnce) {
      attributes.share = GearShare.AccountSharableOnce;
    } else {
      attributes.share = GearShare.AccountSharable;
    }
  }

  if (raw.superiorEqp) {
    attributes.superior = true;
  }
  if (raw.attackSpeed) {
    attributes.attackSpeed = raw.attackSpeed;
  }

  if (raw.specialGrade) {
    attributes.specialGrade = true;
  }
  if (raw.tradeAvailable) {
    attributes.cuttable = raw.tradeAvailable;
  }
  if (raw.CuttableCount) {
    attributes.cuttableCount = raw.CuttableCount;
    attributes.totalCuttableCount = raw.CuttableCount;
  }
  if (raw.accountShareTag) {
    attributes.accountShareTag = true;
  }

  if (raw.setItemID) {
    attributes.setItemId = raw.setItemID;
  }
  if (raw.jokerToSetItem) {
    attributes.lucky = true;
  }
  if (raw.bossReward) {
    attributes.bossReward = true;
  }

  return attributes;
}
