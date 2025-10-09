import {
  GearCapability,
  GearType,
  isDragonGear,
  isMechanicGear,
  isSubWeapon,
} from "@malib/gear";
import { InputGear } from "./schema";

export function getCanPotential(input: InputGear): GearCapability {
  if (input.rawAttributes?.noPotential) {
    return GearCapability.Cannot;
  } else if (input.rawAttributes?.fixedPotential) {
    return GearCapability.Fixed;
  }
  if (
    input.scrollUpgradeableCount ||
    input.rawAttributes?.tucIgnoreForPotential
  ) {
    if (isMechanicGear(input.type) || isDragonGear(input.type)) {
      return GearCapability.Cannot;
    } else {
      return GearCapability.Can;
    }
  }
  return typeSupportsPotential(input.type)
    ? GearCapability.Can
    : GearCapability.Cannot;
}

export function getCanAdditionalPotential(input: InputGear): GearCapability {
  if (input.rawAttributes?.noPotential) {
    return GearCapability.Cannot;
  } else if (input.rawAttributes?.fixedPotential) {
    return GearCapability.Cannot;
  }
  if (
    input.scrollUpgradeableCount ||
    input.rawAttributes?.tucIgnoreForPotential
  ) {
    if (isMechanicGear(input.type) || isDragonGear(input.type)) {
      return GearCapability.Cannot;
    } else {
      return GearCapability.Can;
    }
  }
  return typeSupportsPotential(input.type)
    ? GearCapability.Can
    : GearCapability.Cannot;
}

export function typeSupportsPotential(type: GearType) {
  if (isSubWeapon(type)) {
    return true;
  }
  switch (type) {
    case GearType.soulShield:
    case GearType.demonShield:
    case GearType.katara:
    case GearType.magicArrow:
    case GearType.card:
    case GearType.orb:
    case GearType.dragonEssence:
    case GearType.soulRing:
    case GearType.magnum:
    case GearType.emblem:
      return true;
    default:
      return false;
  }
}
