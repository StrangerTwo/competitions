import delay from "./delay.js";
import Stopwatch from "./stopwatch.js";

const sw = new Stopwatch();
sw.start()

await delay(1200);

const time1 = sw.getTime();
const timeStr = Stopwatch.formatMillis(time1);
console.log(time1, timeStr)

await delay(800);

const split1 = sw.addSplitTime()
console.log(split1)

await delay(500);

const pause1 = sw.pause()
console.log(pause1)

await delay(1000);

const time2 = sw.getTime();
const split2 = sw.addSplitTime();
console.log(time2, split2);

const start1 = sw.start()
console.log(start1)

await delay(500);

const stop1 = sw.stop()
console.log(stop1);

await delay(500);

const time3 = sw.getTime();
const splitTimes1 = sw.getSplitTimes();
console.log(time3, splitTimes1);

const start2 = sw.start()
const splitTimes2 = sw.getSplitTimes();
console.log(start2, splitTimes2);