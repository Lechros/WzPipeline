import { GearCapability, isDragonGear, isMechanicGear } from "@malib/gear";
import { InputGear } from "./schema";

export function getCanStarforce(input: InputGear): GearCapability {
  if (!input.scrollUpgradeableCount) {
    return GearCapability.Cannot;
  }
  if (input.rawAttributes?.onlyUpgrade) {
    return GearCapability.Cannot;
  }
  if (isMechanicGear(input.type) || isDragonGear(input.type)) {
    return GearCapability.Cannot;
  }
  if (input.rawAttributes?.exceptUpgrade) {
    return GearCapability.Fixed;
  }
  return GearCapability.Can;
}
