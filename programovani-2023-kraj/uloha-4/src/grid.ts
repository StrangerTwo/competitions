import { FallingItem } from "./main";

const output = document.getElementById('output');
if (!output) throw new Error('Nelze nic.')

export enum ItemType {
    EMPTY = "",
    YELLOW = "yellow",
    BLUE = "blue"
}

const getBox = (row: number, column: number): HTMLDivElement | null => {
    return output.querySelector(`div[data-row="${row}"][data-column="${column}"]`);
}
const setBoxItem = (row: number, column: number, item: ItemType) => {
    const el = getBox(row, column)
    if (!el) throw new Error('Nelze nic.')
    el.setAttribute("data-item", item);
}

const trySetBox = (row: number, column: number) => {
    const el = getBox(row, column)
    if (!el) return;
    el.setAttribute("data-item", state[row][column]);
}

export const state: ItemType[][] = Array.from(new Array(20)).map(_row => Array.from(new Array(10)).map(_column => ItemType.EMPTY));

export const init = () => {
    Array.from(new Array(20)).forEach((_, row) => {
        Array.from(new Array(10)).forEach((_, column) => {
            output.innerHTML+=`<div data-row="${row}" data-column="${column}"></div>`
        })
    })
}

export const renderFallingItem = (fallingItem: FallingItem) => {
    trySetBox(fallingItem.row - 1, fallingItem.column - 1)
    trySetBox(fallingItem.row - 1, fallingItem.column)
    trySetBox(fallingItem.row - 1, fallingItem.column + 1)
    trySetBox(fallingItem.row - 1, fallingItem.column + 2)
    trySetBox(fallingItem.row, fallingItem.column - 1)
    trySetBox(fallingItem.row, fallingItem.column)
    trySetBox(fallingItem.row, fallingItem.column + 1)
    trySetBox(fallingItem.row, fallingItem.column + 2)
    trySetBox(fallingItem.row + 1, fallingItem.column - 1)
    trySetBox(fallingItem.row + 1, fallingItem.column)
    trySetBox(fallingItem.row + 1, fallingItem.column + 1)
    trySetBox(fallingItem.row + 1, fallingItem.column + 2)

    if (fallingItem.type == ItemType.YELLOW) {
        setBoxItem(fallingItem.row, fallingItem.column, fallingItem.type)
        setBoxItem(fallingItem.row, fallingItem.column + 1, fallingItem.type)
        setBoxItem(fallingItem.row + 1, fallingItem.column, fallingItem.type)
        setBoxItem(fallingItem.row + 1, fallingItem.column + 1, fallingItem.type)
    } else {
        setBoxItem(fallingItem.row, fallingItem.column, fallingItem.type)
    }
}

export const doesFallingItemHit = (fallingItem: FallingItem) => {
    if (fallingItem.type == ItemType.YELLOW) {
        if (fallingItem.row === 18 || state[fallingItem.row][fallingItem.column] || state[fallingItem.row][fallingItem.column + 1] || state[fallingItem.row + 1][fallingItem.column] || state[fallingItem.row + 1][fallingItem.column + 1] || state[fallingItem.row + 2][fallingItem.column] || state[fallingItem.row + 2][fallingItem.column + 1]) {
            return true;
        }
    } else {
        if (fallingItem.row === 19 || state[fallingItem.row][fallingItem.column] || state[fallingItem.row + 1][fallingItem.column]) {
            return true;
        }
    }
    return false;
}

export const saveOnHit = (fallingItem: FallingItem) => {
    if (fallingItem.type == ItemType.YELLOW) {
        state[fallingItem.row][fallingItem.column] = fallingItem.type
        state[fallingItem.row][fallingItem.column + 1] = fallingItem.type
        state[fallingItem.row + 1][fallingItem.column] = fallingItem.type
        state[fallingItem.row + 1][fallingItem.column + 1] = fallingItem.type
        setBoxItem(fallingItem.row, fallingItem.column, fallingItem.type)
        setBoxItem(fallingItem.row, fallingItem.column + 1, fallingItem.type)
        setBoxItem(fallingItem.row + 1, fallingItem.column, fallingItem.type)
        setBoxItem(fallingItem.row + 1, fallingItem.column + 1, fallingItem.type)
    } else {
        state[fallingItem.row][fallingItem.column] = fallingItem.type
        setBoxItem(fallingItem.row, fallingItem.column, fallingItem.type)
    }
}

export const checkRows = () => {
    const rowsAndIndexes = state.map((row, i) => [row, i] as [ItemType[], number]).filter(([row, _i]) => !row.includes(ItemType.EMPTY))

    if (rowsAndIndexes.length == 0) return false;
    for (const [row, index] of rowsAndIndexes) {
        console.log(`Mažu řádek č. ${index + 1}`)
        state.splice(index, 1)
        state.unshift(state[0].map(_x => ItemType.EMPTY))
    }
    rerender()
    return true;
}

const rerender = () => {
    Array.from(new Array(20)).forEach((_, row) => {
        Array.from(new Array(10)).forEach((_, column) => {
            trySetBox(row, column)
        })
    })
}