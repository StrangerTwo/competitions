import { closeSync, open, openSync, readFileSync, writeSync } from "fs";
import { readLine, readLineInt } from "../lib/file";

const lines = readFileSync("docs/Kasiopea/C.txt").toString().split("\n");

const T = readLineInt(lines);
const file = openSync("docs/Kasiopea/C-reseni.txt", "w")

for (const uloha of [...new Array(T).keys()]) {
    const N = readLineInt(lines);
    var P = readLine(lines).split(" ").map(x => parseInt(x));

    console.log(N, P.length)

    const indexes = new Map<number, number[]>();

    console.log('preparing')
    P.forEach((x, i) => {
        if (indexes.get(x))
            (indexes.get(x) as number[]).push(i)
        else
            indexes.set(x, [i])
    })
    console.log('done')

    var distance = 0;
    var min = 0;
    var i = 0;      // start na nule
    for (const kolikataPonozka of [...new Array(N).keys()]) {
        const a = P[i];
        P[i] = 0;       // sebrano

        const j = (indexes.get(a) as number[])[1] // najdu druhou ponozku
        distance += j-i; // dojdu ke druhe ponozce
        P[j] = 0;       // sebrano

        i = findIndex(P, min); // najdu novou pomozku
        min = i;

        if (kolikataPonozka && kolikataPonozka % 20000 === 0) console.log(Math.round(kolikataPonozka/N*100), '%', '(', i, ')')
        if (i > -1)
            distance += Math.abs(j-i); // dojdu k prvni ponozce
    }

    writeSync(file, `${distance}\n`);
}

function findIndex(arr: number[], start: number) {
    var i = start;
    while (i < arr.length) {
        if (arr[i] > 0) return i;
        i++;
    }
    return -1
}

closeSync(file);

console.log('Done')