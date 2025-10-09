import { GearGender, GearType } from "@malib/gear";
import { getGender } from "./gender";
import { InputGear } from "./schema";

test("Test gender", () => {
  const gearData: InputGear = {
    meta: {
      id: 1040122,
      version: 1,
    },
    name: "블랙 네오스",
    icon: "1040122",
    type: GearType.coat,
    req: {
      job: 1,
      level: 100,
    },
    attributes: {},
    baseOption: {
      str: 6,
      dex: 2,
      armor: 105,
    },
    rawAttributes: {
      setItemID: 367,
    },
    scrollUpgradeableCount: 8,
  };
  const expected = GearGender.Male;

  const actual = getGender(gearData);

  expect(actual).toBe(expected);
});
