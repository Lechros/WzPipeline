import {
  GearCapability,
  GearType,
  isAccessory,
  isArmor,
  isShield,
  isWeapon,
} from "@malib/gear";
import { InputGear } from "./schema";

export function getCanAddOption(input: InputGear): GearCapability {
  if (input.rawAttributes?.exUpgradeBlock) {
    return GearCapability.Cannot;
  }
  if (input.rawAttributes?.exUpgradeChangeBlock) {
    return GearCapability.Fixed;
  }
  if (input.rawAttributes?.setExtraOption) {
      return GearCapability.Can;
  }
  return typeSupportsAddOption(input.type)
    ? GearCapability.Can
    : GearCapability.Cannot;
}

export function typeSupportsAddOption(type: GearType): boolean {
  if (isWeapon(type)) {
    return true;
  }
  if (isArmor(type)) {
    return !isShield(type);
  }
  if (isAccessory(type)) {
    return ![GearType.ring, GearType.shoulder].includes(type);
  }
  if (type === GearType.pocket) {
    return true;
  }
  return false;
}
