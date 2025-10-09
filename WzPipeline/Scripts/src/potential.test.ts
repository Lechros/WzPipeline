import { GearCapability, GearType } from "@malib/gear";
import {
  getCanAdditionalPotential,
  getCanPotential,
  typeSupportsPotential,
} from "./potential";
import { InputGear } from "./schema";

describe("getCanPotential", () => {
  it("returns Can for tuc > 0", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanPotential(input)).toBe(GearCapability.Can);
  });

  it("returns true for tuc === 0 and tucIgnoreForPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      rawAttributes: {
        tucIgnoreForPotential: 1,
      },
    };

    expect(getCanPotential(input)).toBe(GearCapability.Can);
  });

  it.each([
    GearType.machineEngine,
    GearType.machineArms,
    GearType.machineLegs,
    GearType.machineBody,
    GearType.machineTransistors,
    GearType.dragonMask,
    GearType.dragonPendant,
    GearType.dragonWings,
    GearType.dragonTail,
  ])("returns false for tuc > 0 and type === %p", (type) => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: type,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanPotential(input)).toBe(GearCapability.Cannot);
  });

  it("returns false for noPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      rawAttributes: {
        noPotential: 1,
      },
      scrollUpgradeableCount: 1,
    };

    expect(getCanPotential(input)).toBe(GearCapability.Cannot);
  });

  it("returns Fixed for fixedPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      rawAttributes: {
        fixedPotential: 1,
      },
      scrollUpgradeableCount: 1,
    };

    expect(getCanPotential(input)).toBe(GearCapability.Fixed);
  });

  it("returns by type for other cases", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.katara,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanPotential(input)).toBe(GearCapability.Can);
  });
});

describe("getCanAdditionalPotential", () => {
  it("returns Can for tuc > 0", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Can);
  });

  it("returns true for tuc === 0 and tucIgnoreForPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
      rawAttributes: {
        tucIgnoreForPotential: 1,
      },
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Can);
  });

  it.each([
    GearType.machineEngine,
    GearType.machineArms,
    GearType.machineLegs,
    GearType.machineBody,
    GearType.machineTransistors,
    GearType.dragonMask,
    GearType.dragonPendant,
    GearType.dragonWings,
    GearType.dragonTail,
  ])("returns false for tuc > 0 and type === %p", (type) => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: type,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Cannot);
  });

  it("returns false for noPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      rawAttributes: {
        noPotential: 1,
      },
      scrollUpgradeableCount: 1,
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Cannot);
  });

  it("returns Cannot for fixedPotential === 1", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
      rawAttributes: {
        fixedPotential: 1,
      },
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Cannot);
  });

  it("returns by type for other cases", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.katara,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanAdditionalPotential(input)).toBe(GearCapability.Can);
  });
});

it.each([
  GearType.soulShield,
  GearType.demonShield,
  GearType.katara,
  GearType.magicArrow,
  GearType.card,
  GearType.orb,
  GearType.dragonEssence,
  GearType.soulRing,
  GearType.magnum,
  GearType.emblem,
  GearType.shield,
  GearType.katara,
  GearType.jewel,
])(
  "is true for canPotential === None with scrollTotalUpgradeableCount === 0 if type is %s",
  (type) => {
    expect(typeSupportsPotential(type)).toBe(true);
  }
);
