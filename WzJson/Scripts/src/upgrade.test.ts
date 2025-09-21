import { GearCapability, GearType } from "@malib/gear";
import { InputGear } from "./schema";
import { getCanScroll } from "./upgrade";

describe("getCanScroll", () => {
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

    expect(getCanScroll(input)).toBe(GearCapability.Cannot);
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

    expect(getCanScroll(input)).toBe(GearCapability.Cannot);
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

    expect(getCanScroll(input)).toBe(GearCapability.Fixed);
  });

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

    expect(getCanScroll(input)).toBe(GearCapability.Can);
  });
});
