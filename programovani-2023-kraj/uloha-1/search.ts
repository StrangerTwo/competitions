const search = (maze: string[][], visited: boolean[][], x: number, y: number, currentWord = ""): string[] => {
    if (!currentWord && maze[x][y] !== ' ' && maze[x][y].toUpperCase() !== maze[x][y]) {
        // start from the start of the word, we will come back later
        const wordStart = findWordStart(maze, maze.map(line => line.map(x => false)), x, y);
        if (!wordStart) throw new Error('Slovo nemá začátek')
        return search(maze, visited, wordStart.x, wordStart.y);
    }
    if (visited[x][y]) return [];
    visited[x][y] = true;
    if (maze[x][y] !== ' ')
        currentWord += maze[x][y];  // append current letter
    else
        currentWord = ""

    const locations = getAcessibleLocations(maze, visited, x, y);

    const words: string[] = [];

    const letter = locations.find(location => maze[location.x][location.y] !== ' ')

    // console.log(x, y, locations)
    for (const location of locations) {
        words.push(...search(maze, visited, location.x, location.y, currentWord));
    }
   
    if (currentWord && !letter) {
        console.log(x, y, currentWord)
        words.push(currentWord)
    }
    return words.sort((a, b) => a.localeCompare(b));
}

export default search;

interface Location {
    x: number,
    y: number,
}

const getAcessibleLocations = (maze: string[][], visited: boolean[][], x: number, y: number): Location[] => {
    const result: Location[] = [];

    if (x > 0 && maze[x-1][y] !== "#") {
        result.push({x: x-1, y: y});
    }
    if (y > 0 && maze[x][y-1] !== "#") {
        result.push({x: x, y: y-1});
    }
    if (x <= maze.length && maze[x+1][y] !== "#") {
        result.push({x: x+1, y: y});
    }
    if (y <= maze[0].length && maze[x][y+1] !== "#") {
        result.push({x: x, y: y+1});
    }
    return result.filter(location => !visited[location.x][location.y]).sort((a, b) => {
        if (maze[a.x][a.y] !== " ") // if its continuin a word, prioritize it
            return -1;
        return 1;
    });
}

const findWordStart = (maze: string[][], visited: boolean[][], x: number, y: number): Location | null => {
    visited[x][y] = true;
    if (maze[x][y].toUpperCase() == maze[x][y]) return {x: x, y: y};

    const locations = getAcessibleLocations(maze, visited, x, y);
    for (const location of locations.filter(location => maze[location.x][location.y] !== ' ')) {
        const start = findWordStart(maze, visited, location.x, location.y);
        if (start) return start;
    }
    return null;
}
