const getPixels = (await import('get-pixels')).default

const _parseImg = async (path: string): Promise<number[][]> => {
    return new Promise((resolve, reject) => {
        getPixels(path, function(err, pixels) {
            if(err) {
                console.log("Bad image path")
                reject()
                return
            }
            const [width, height, channels] = pixels.shape;
            
            const data: number[][] = Array.from(new Array(height)).map(row => Array.from(new Array(width)).map(x => 0));
            console.log(path, "width", width, "height", height, "channels", channels)
            // console.log(pixels.data.slice(0,4))
            // console.log(data.length, data[0].length, pixels.data.length, width * height * channels)
            pixels.data.forEach((value, index) => {
                // console.log(index, Math.floor(index / width), index % width)
                switch (index % 4) {
                    case 1:
                        data[Math.floor(index / width / 4)][index / 4 % width] += value * 255 * 255 * 255
                    case 2:
                        data[Math.floor(index / width / 4)][index / 4 % width] += value * 255 * 255
                    case 3:
                        data[Math.floor(index / width / 4)][index / 4 % width] += value * 255
                    case 0:
                        data[Math.floor(index / width / 4)][index / 4 % width] += value
                }
            })
    
            resolve(data);
        })
    })
}


const parseImg = async (imagePaths: string[]): Promise<number[][][]> => {
    const result: number[][][] = [];

    for (const imagePath of imagePaths) {
        result.push(await _parseImg(imagePath))
    }

    return result;
}

export default parseImg;