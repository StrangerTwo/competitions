import { closeSync, open, openSync, readFileSync, writeSync } from "fs";
import { readLine, readLineInt } from "../lib/file";

const lines = readFileSync("docs/Kasiopea/A.txt").toString().split("\n");

const N = readLineInt(lines);
const file = openSync("docs/Kasiopea/A-reseni.txt", "w")

for (const uloha of [...new Array(N).keys()]) {
    const S = readLineInt(lines);
    const V = readLineInt(lines);
    const x1 = readLineInt(lines);
    const x2 = readLineInt(lines);

    const x = Math.min(x1 + x2, S-x1 + S-x2);
    const result = x + V;

    writeSync(file, `${result}\n`);
}

closeSync(file);