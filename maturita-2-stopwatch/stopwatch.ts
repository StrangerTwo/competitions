class Stopwatch {

    startTime: number | null = null;
    stopTime: number | null = null;
    splitTimes: number[] = [];
    pauseTimes: number[] = [];

    isStarted(): boolean {
        return this.startTime !== null;
    }

    isPaused(): boolean {
        return this.pauseTimes.length % 2 == 1;
    }

    isStopped(): boolean {
        return this.stopTime !== null;
    }

    isRunning(): boolean {
        return this.isStarted() && !this.isStopped();
    }

    getSplitTimes(): number[] {
        return this.splitTimes;
    }

    start(): boolean {
        if (this.isRunning() && !this.isPaused()) {
            return false;
        }

        if (this.isPaused()) {
            this.pauseTimes.push(Date.now())
            return true;
        }

        this.startTime = Date.now();
        this.splitTimes = [];
        this.pauseTimes = [];
        return true;
    }

    pause(): boolean {
        if (this.isPaused()) return false;

        this.pauseTimes.push(Date.now())
        return true;
    }

    stop(): boolean {
        if (!this.isRunning()) return false;
        if (this.isPaused()) this.start()

        this.stopTime = Date.now()
        return true;
    }

    getTime(): number {
        if (!this.isStarted()) throw new Error("Timer has not been started yet!");

        const untilTime = this.stopTime ?? Date.now();
        var time = untilTime - (this.startTime as number)
        for (const delay of this.getPauseDurations()) {
            time -= delay
        }
        return time;
    }

    getPauseDurations(): number[] {
        var from: number | null = null;

        const currentPauseTimes = this.isPaused() ? [...this.pauseTimes, Date.now()] : this.pauseTimes

        const result: number[] = [];
        for (const time of currentPauseTimes) {
            if (!from) {
                from = time;
                continue;
            }
            result.push(time - from);
            from = null
        }
        return result;
    }

    addSplitTime(): number {
        if (this.isStopped()) throw new Error("Timer has already been stopped");

        const time = this.getTime();
        this.splitTimes.push(time);
        return time;

    }

    static formatMillis(millis: number): string {
        const hours = Math.floor(millis / (60 * 60 * 1000))
        const minutes = Math.floor((millis % (60 * 60 * 1000)) / (60 * 1000))
        const seconds = Math.floor((millis % (60 * 1000)) / (1000))
        const milliseconds = millis % 1000

        return `${this.padStart(2, hours)}:${this.padStart(2, minutes)}:${this.padStart(2, seconds)}.${this.padStart(3, milliseconds)}`
    }

    static padStart(max: number, number: number): string {
        return `${number}`.padStart(max, "0")
    }

}

export default Stopwatch;