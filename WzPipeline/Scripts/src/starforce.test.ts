import { GearCapability, GearType } from "@malib/gear";
import { InputGear } from "./schema";
import { getCanStarforce } from "./starforce";

describe("getCanStarforce", () => {
  it("returns Cannot for tuc === undefined", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: undefined,
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Cannot);
  });

  it("returns Cannot for tuc === 0", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.cap,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 0,
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Cannot);
  });

  it("returns Cannot for onlyUpgrade === 1", () => {
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
        onlyUpgrade: 1,
      },
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Cannot);
  });

  it("returns Fixed for exceptUpgrade === 1", () => {
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
        exceptUpgrade: 1,
      },
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Fixed);
  });

  it("returns Cannot for mechanic gear", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.machineArms,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Cannot);
  });

  it("returns Cannot for dragon gear", () => {
    const input: InputGear = {
      meta: {
        id: 0,
        version: 1,
      },
      name: "",
      type: GearType.dragonPendant,
      req: {},
      attributes: {},
      scrollUpgradeableCount: 1,
    };

    expect(getCanStarforce(input)).toBe(GearCapability.Cannot);
  });

  it("returns Can for other cases", () => {
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

    expect(getCanStarforce(input)).toBe(GearCapability.Can);
  });
});
