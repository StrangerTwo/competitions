export const readLine = (lines: Array<string>) => {
    return lines.shift() as string;
}

export const readLineInt = (lines: Array<string>) => {
    return parseInt(readLine(lines));
}