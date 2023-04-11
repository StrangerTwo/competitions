import fs from 'fs'
import inquireFilePathPrompt from 'inquirer-file-path'
const inquirer = (await import('inquirer')).default

fs.writeFileSync('test.json', JSON.stringify({lol: 'xd'}));

inquirer.registerPrompt('filePath', inquireFilePathPrompt)
inquirer
    .prompt([
      {
        type: 'filePath',
        name: 'filePath',
        message: 'Vyber cestu k souboru',
        basePath: '.'
      },
    ])
    .then(async (answers) => {
      const {filePath} = answers;

        console.log(filePath)
    });