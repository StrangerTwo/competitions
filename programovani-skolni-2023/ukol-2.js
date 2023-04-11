const inquirer = require('inquirer')
const inquirerPrompt = require('inquirer-file-path')
var getPixels = require("get-pixels")
const fs = require('fs')


async function getHeight(path, x, y) {
    console.log(path)
    return new Promise((resolve, reject) => {
        getPixels(path, function(err, pixels) {
            if(err) {
            console.log("Bad image path")
            return
            }
            const [width, height, channels] = pixels.shape;
        
            const from = y * width * channels + x * channels;
            const to = from + channels
        
            const pixel = pixels.data.slice(from, to)
        
            resolve(pixel[0])
        })
    })
}
inquirer.registerPrompt('folderPath', inquirerPrompt);

async function run() {

    inquirer
    .prompt([
      {
        type: 'folderPath',
        name: 'folderpath',
        message: 'Vyber cestu k souboru',
        basePath: '.'
      },
    ])
    .then(async (answers) => {
      const {folderpath} = answers;
      
        const file = fs.readFileSync(folderpath).toString();

        for (const line of file.split('\n')) {

            const x = parseInt(line.split(' ')[0])

        const y = parseInt(line.split(' ')[1])
        const name = line.split(' ')[2]
            
            const height = await getHeight(`mapy/${name}.png`, x, y)

            console.log(`${x} ${y} ${height} ${name}`)
        }
    });
}

run()
