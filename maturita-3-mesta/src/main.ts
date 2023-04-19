import getRoadsToUpgrade from './road';
import './style.css'

const btn = document.querySelector<HTMLButtonElement>('#btn-1');
const div = document.querySelector('#content')
const svg = document.querySelector('#svg')
const tooltip = document.querySelector('#tooltip')
if (!btn || !div || !svg ||!tooltip) throw new Error("Nelze nic.")

export interface Road {
    id: number,
    start: number,
    end: number,
    travelTime: number,
    upgradePrice: number,
}

interface Data {
    roads: Array<Road>
    towns: Array<{
        id: number,
        inhabitants: number
        name: string,
        x: number
        y: number
        cx: number,
        cy: number,
        r: number
    }>
}

btn.onclick = async () => {
    div.classList.add("start");
    console.log('Start');

    tooltip.innerHTML = `Loading...`
    const data: Data = await fetch("https://maturita.delta-www.cz/prakticka/2023-map/mapData").then(res => res.json())
    tooltip.innerHTML = ``

    console.log(data.roads)

    const fullHeight = svg.getBoundingClientRect().height - 20;
    const fullWidth = svg.getBoundingClientRect().width - 20;

    const maxX = data.towns.sort((a, b) => b.x - a.x)[0].x
    const maxY = data.towns.sort((a, b) => b.y - a.y)[0].y

    const upgradedRoads: Road[] = getRoadsToUpgrade(data.roads);

    for (const town of data.towns) {
        town.cx = 10 + town.x / maxX * fullWidth;
        town.cy = 10 + town.y / maxY * fullHeight;
        town.r = Math.log(town.inhabitants / 3000) * 5;
    }

    for (const road of data.roads) {
        const townA = data.towns.find(x => x.id == road.start);
        const townB = data.towns.find(x => x.id == road.end);
        if (!townA || !townB) return;
        var isUpgraded = !!upgradedRoads.find(x => x.id == road.id);
        svg.innerHTML += `<line x1="${townA.cx}" y1="${townA.cy}" x2="${townB.cx}" y2="${townB.cy}" stroke="${isUpgraded ? "black": "grey"}" stroke-width="${isUpgraded ? 4 : 2}" />`
    }

    for (const town of data.towns) {
        svg.innerHTML += `<circle data-id="${town.id}" cx="${town.cx}" cy="${town.cy}" r="${town.r}" stroke="black" stroke-width="2" fill="red" />`
    }
    for (const circle of Array.from(svg.querySelectorAll('circle'))) {
        circle.onmouseenter = (e: MouseEvent) => {
            const circle = e.currentTarget as any;
            const id = parseInt(circle.getAttribute('data-id'));

            const town = data.towns.find(x => x.id == id)
            if (!town) return;
            tooltip.innerHTML = `${town.id}: ${town.name} (${town.inhabitants})`
        }
    }
}