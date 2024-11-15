import { closeSync, open, openSync, readFileSync, writeSync } from "fs";
import { readLine, readLineInt } from "../lib/file";

const lines = readFileSync("docs/Kasiopea/D.txt").toString().split("\n");

const T = readLineInt(lines);
const file = openSync("docs/Kasiopea/D-reseni.txt", "w")
var cesty: Map<number, [number, number][]>;

for (const uloha of [...new Array(T).keys()]) {
    const N = readLineInt(lines);
    cesty = new Map();
    for (const i of [...new Array(N-1).keys()]) {
        const x = readLine(lines).split(" ").map(x => parseInt(x)) as [number, number, number];
        addCesta(...x)
        addCesta(x[1], x[0], x[2])
    }

    console.log('here')
    const v = BFS(1).at(-1)!;
    console.log('here')
    const trail = BFS(v);
    console.log('here')

    const center = trail[Math.floor(trail.length / 2)];

    var distance = 0;
    const queue: {
        x: number,
        a: number,
    }[] = [{
        x: center,
        a: -1,
    }]

    while (queue.length) {
        const x = queue.shift()!;

        for (const [y, dist] of cesty.get(x.x)!) {
            if (y === x.a) continue;
            distance += dist;
            queue.push({
                a: x.x,
                x: y
            })
        }
    }

    writeSync(file, `${distance}\n`);
}

function addCesta(x: number, y: number, dist: number) {
    if (cesty.get(x))
        cesty.get(x)!.push([y, dist]);
    else
        cesty.set(x, [[y, dist]]);
}

closeSync(file);

console.log('Done')

function BFS(x: number): number[] {
    var queue: {
        dist: number,
        x: number,
        trail: number[]
    }[] = [{
        dist: 0,
        x,
        trail: [x]
    }];

    var result: number[];
    while (true) {
        const x = queue.shift()!;
        for (const [y, dist] of (cesty.get(x.x) as [number, number][])) {
            if (y === x.trail.at(-2)) continue;

            const obj: {
                dist: number,
                x: number,
                trail: number[]
            } = {
                x: y,
                dist: x.dist + dist,
                trail: [...x.trail, y]
            }
            const fi = queue.findIndex(x => x.dist > obj.dist);
            const i = fi === -1 ? 0 : fi;
            queue = [...queue.slice(0, i), obj, ...queue.slice(i)]
        }

        if (queue.length < 5)
        console.log(queue)
        if (queue.length === 0) {
            result = x.trail;
            break;
        }
    }
    return result;
}

