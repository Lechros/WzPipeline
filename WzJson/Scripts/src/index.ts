import { GearData } from "@malib/gear";
import { process } from "./process";
import { InputGear } from "./schema";

type Callback = (error: Error | string | null, result: GearData[]) => void;

function main(callback: Callback, gears: InputGear[]) {
  callback(null, gears.map(process));
}

export default main;
