import { GearCapability } from '@malib/gear';
import { InputGear } from './schema';

export function getCanScroll(input: InputGear): GearCapability {
  if (!input.scrollUpgradeableCount) {
    return GearCapability.Cannot;
  }
  if (input.rawAttributes?.exceptUpgrade) {
    return GearCapability.Fixed;
  }
  return GearCapability.Can;
}
