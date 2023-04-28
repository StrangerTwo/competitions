import fs from 'fs'
import getPath from './getPath.js'
import search from './search.js';

const filePath = await getPath()

const fileContents = fs.readFileSync(filePath).toString().trim();

const header = fileContents.split("\n")[0];

const maze = fileContents.split('\n').slice(1).map(line => line.split('').filter(x => x !== '\r'));
const visited = maze.map(line => line.map(x => false)); // get a boolean copy of visited locations

const entryRowIndex = maze.findIndex(line => line[0] !== "#");

// console.log("Entry", [entryRowIndex, 0])
const words = search(maze, visited, entryRowIndex, 0);

console.log(header);
console.log("Nalezena slova:", words)