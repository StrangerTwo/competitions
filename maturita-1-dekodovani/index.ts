import fs from 'fs'
import getPath from './getPath.js'
import Jimp from "jimp"

// fs.writeFileSync('test.json', JSON.stringify({lol: 'xd'}));

// const filePath = "squares.dat"

const filePath = await getPath()
console.log(filePath)

const buffer = fs.readFileSync(filePath);

var qrStr = ""

buffer.forEach(byte => {
  // console.log(byte)
  qrStr += (byte >>> 0).toString(2).padStart(8, "0")
})

// console.log(qrStr)

const imageData: number[][] = []

var temp: number[] = []
qrStr.split('').forEach((x, i) => {
  temp.push(x == "1" ? 0x000000FF : 0xFFFFFFFF)
  if (temp.length == 35) {
    imageData.push(temp);
    temp = []
  }
})

let image = new Jimp(35, 35, function (err, image) {
  if (err) throw err;


  imageData.forEach((row, i) => {
    row.forEach((x, j) => {
      image.setPixelColor(x, j, i);
    })
  })

  image.write('qr.png', (err) => {
    if (err) throw err;
  });
});