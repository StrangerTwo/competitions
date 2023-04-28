
import inquireDirectoryPrompt from 'inquirer-directory'
const inquirer = (await import('inquirer')).default

inquirer.registerPrompt('folderPath', inquireDirectoryPrompt)

const getFolder = () => {
    return inquirer
        .prompt([
        {
            type: 'folderPath',
            name: 'folderPath',
            message: 'Vyber cestu ke složce s obrázky',
            basePath: '.'
        },
        ])
        .then(answers => answers.folderPath);
}

export default getFolder;