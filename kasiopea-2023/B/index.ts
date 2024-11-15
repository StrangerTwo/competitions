import { closeSync, open, openSync, readFileSync, writeSync } from "fs";
import { readLine, readLineInt } from "../lib/file";

const lines = readFileSync("docs/Kasiopea/B.txt").toString().split("\n");

const T = readLineInt(lines);
const file = openSync("docs/Kasiopea/B-reseni.txt", "w")

for (const uloha of [...new Array(T).keys()]) {
    const N = readLineInt(lines);
    const days = readLine(lines).split(" ").map(x => parseInt(x));

    var record = days[0];
    var records = 1;

    for (const day of days.slice(1)) {
        if (day > record) {
            record = day;
            records++;
        }
    }

    writeSync(file, `${records}\n`);
}

closeSync(file);

console.log('Done')