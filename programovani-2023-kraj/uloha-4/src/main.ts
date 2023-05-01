import { ItemType, checkRows, doesFallingItemHit, init, renderFallingItem, saveOnHit } from './grid'
import './style.css'

export interface FallingItem {
    type: ItemType,
    row: number;
    column: number;
}

var fallingItem: FallingItem | null = null

const generate = (): FallingItem => {
    const type = Math.floor(Math.random() * 2) ? ItemType.YELLOW : ItemType.BLUE
    return {
        type: type,
        row: 0,
        column: Math.floor(Math.random() * (type == ItemType.BLUE ? 10 : 9))
    }
}

setInterval(() => {
    if (!fallingItem) {
        if (checkRows()) {
            return;
        }
        fallingItem = generate()
        renderFallingItem(fallingItem)
    } else {
        fallingItem.row++;
        renderFallingItem(fallingItem);

        if (doesFallingItemHit(fallingItem)) {
            saveOnHit(fallingItem)
            fallingItem = null;
        }
    }


}, 500)

window.onkeydown = (e) => {
    if (!fallingItem) return;
    if (e.key == "a") {
        fallingItem.column--;
        if (doesFallingItemHit(fallingItem)) {
            fallingItem.column++;
            return;
        }
        fallingItem.column = Math.max(0, fallingItem.column);
        renderFallingItem(fallingItem)
    }
    if (e.key == "d") {
        fallingItem.column++;
        if (doesFallingItemHit(fallingItem)) {
            fallingItem.column--;
            return;
        }
        fallingItem.column = Math.min(fallingItem.type == ItemType.YELLOW ? 8 : 9, fallingItem.column);
        
        renderFallingItem(fallingItem)
    }
}

init()