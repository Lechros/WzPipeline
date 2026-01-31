import { GearCapability, GearType } from "@malib/gear";
import { getCanAddOption, typeSupportsAddOption } from "./addOption";
import { InputGear } from "./schema";

test("exUpgradeBlock returns Cannot", () => {
  const input: InputGear = {
    id: 0,
    version: 2,
    icon: "",
    name: "",
    type: GearType.cap,
    req: {},
    attributes: {},
    rawAttributes: {
      exUpgradeBlock: 1,
    },
  };

  const actual = getCanAddOption(input);

  expect(actual).toBe(GearCapability.Cannot);
});

test("exUpgradeChangeBlock returns Fixed", () => {
  const input: InputGear = {
    id: 0,
    version: 2,
    icon: "",
    name: "",
    type: GearType.cap,
    req: {},
    attributes: {},
    rawAttributes: {
      exUpgradeChangeBlock: 1,
    },
  };

  const actual = getCanAddOption(input);

  expect(actual).toBe(GearCapability.Fixed);
});

test("exUpgradeBlock, exUpgradeChangeBlock returns Cannot", () => {
  const input: InputGear = {
    id: 0,
    version: 2,
    icon: "",
    name: "",
    type: GearType.cap,
    req: {},
    attributes: {},
    rawAttributes: {
      exUpgradeBlock: 1,
      exUpgradeChangeBlock: 1,
    },
  };

  const actual = getCanAddOption(input);

  expect(actual).toBe(GearCapability.Cannot);
});

test("no add option related props calculates by gear type", () => {
  const input: InputGear = {
    id: 0,
    version: 2,
    icon: "",
    name: "",
    type: GearType.cap,
    req: {},
    attributes: {},
  };

  const actual = getCanAddOption(input);

  expect(actual).toBe(GearCapability.Can);
});

test.each(
  getGearTypeContains([
    GearType.cap,
    GearType.coat,
    GearType.longcoat,
    GearType.pants,
    GearType.shoes,
    GearType.glove,
    GearType.cape,
    GearType.faceAccessory,
    GearType.eyeAccessory,
    GearType.earrings,
    GearType.pendant,
    GearType.belt,
    GearType.pocket,
    GearType.shiningRod,
    GearType.tuner,
    GearType.breathShooter,
    GearType.longSword2,
    GearType.soulShooter,
    GearType.desperado,
    GearType.energySword,
    GearType.espLimiter,
    GearType.chain,
    GearType.magicGauntlet,
    GearType.ritualFan,
    GearType.ohSword,
    GearType.ohAxe,
    GearType.ohBlunt,
    GearType.dagger,
    GearType.cane,
    GearType.wand,
    GearType.staff,
    GearType.thSword,
    GearType.chakram,
    GearType.thAxe,
    GearType.thBlunt,
    GearType.spear,
    GearType.polearm,
    GearType.bow,
    GearType.crossbow,
    GearType.claw,
    GearType.knuckle,
    GearType.gun,
    GearType.dualBowguns,
    GearType.handCannon,
    GearType.heavySword,
    GearType.longSword,
    GearType.gauntletRevolver,
    GearType.ancientBow,
  ])
)("supportsAddOption(type=%d) returns %p", (gearType, expected) => {
  const actual = typeSupportsAddOption(gearType);

  expect(actual).toBe(expected);
});

export function getAllGearTypes() {
  return [
    GearType.cap,
    GearType.faceAccessory,
    GearType.eyeAccessory,
    GearType.earrings,
    GearType.coat,
    GearType.longcoat,
    GearType.pants,
    GearType.shoes,
    GearType.glove,
    GearType.cape,
    GearType.ring,
    GearType.pendant,
    GearType.belt,
    GearType.medal,
    GearType.shoulder,
    GearType.pocket,
    GearType.badge,
    GearType.android,
    GearType.machineHeart,
    GearType.shield,
    GearType.emblem,
    GearType.powerSource,
    GearType.shiningRod,
    GearType.tuner,
    GearType.breathShooter,
    GearType.longSword2,
    GearType.soulShooter,
    GearType.desperado,
    GearType.energySword,
    GearType.espLimiter,
    GearType.chain,
    GearType.magicGauntlet,
    GearType.ritualFan,
    GearType.ohSword,
    GearType.ohAxe,
    GearType.ohBlunt,
    GearType.dagger,
    GearType.katara,
    GearType.cane,
    GearType.wand,
    GearType.staff,
    GearType.thSword,
    GearType.chakram,
    GearType.thAxe,
    GearType.thBlunt,
    GearType.spear,
    GearType.polearm,
    GearType.bow,
    GearType.crossbow,
    GearType.claw,
    GearType.knuckle,
    GearType.gun,
    GearType.shovel,
    GearType.pickaxe,
    GearType.dualBowguns,
    GearType.handCannon,
    GearType.heavySword,
    GearType.longSword,
    GearType.gauntletRevolver,
    GearType.ancientBow,
    GearType.soulShield,
    GearType.demonShield,
    GearType.magicArrow,
    GearType.card,
    GearType.medallion,
    GearType.rosary,
    GearType.ironChain,
    GearType.magicBook1,
    GearType.magicBook2,
    GearType.magicBook3,
    GearType.arrowFletching,
    GearType.bowThimble,
    GearType.daggerScabbard,
    GearType.charm,
    GearType.orb,
    GearType.dragonEssence,
    GearType.soulRing,
    GearType.magnum,
    GearType.wristBand,
    GearType.farSight,
    GearType.powderKeg,
    GearType.mass,
    GearType.document,
    GearType.magicMarble,
    GearType.arrowhead,
    GearType.jewel,
    GearType.powderKeg2,
    GearType.controller,
    GearType.foxMarble,
    GearType.chessPiece,
    GearType.transmitter,
    GearType.charge,
    GearType.magicWing,
    GearType.pathOfAbyss,
    GearType.relic,
    GearType.fanTassel,
    GearType.bracelet,
    GearType.weaponBelt,
    GearType.ornament,
    GearType.hexSeeker,
    GearType.sacredJewel,
    GearType.petEquip,
    GearType.machineEngine,
    GearType.machineArms,
    GearType.machineLegs,
    GearType.machineBody,
    GearType.machineTransistors,
    GearType.dragonMask,
    GearType.dragonPendant,
    GearType.dragonWings,
    GearType.dragonTail,
  ];
}

export function getGearTypeContains(types: GearType[]): [GearType, boolean][] {
  const set = new Set(types);
  return getAllGearTypes().map((type) => [type, set.has(type)]);
}
