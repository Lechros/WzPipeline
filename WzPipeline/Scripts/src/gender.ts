import { GearGender, GearType } from "@malib/gear";
import { InputGear } from "./schema";

export function getGender(input: InputGear): GearGender | undefined {
  switch (input.type) {
    case GearType.emblem:
    case GearType.powerSource:
    case GearType.jewel:
      return undefined;
  }
  const value = Math.floor(input.meta.id / 1000) % 10;
  switch (value) {
    case GearGender.Male:
      return GearGender.Male;
    case GearGender.Female:
      return GearGender.Female;
    default:
      return undefined;
  }
}
