import inquireFilePathPrompt from 'inquirer-file-path'
const inquirer = (await import('inquirer')).default
inquirer.registerPrompt('filePath', inquireFilePathPrompt)


const getPath = () => inquirer
    .prompt([
        {
            type: 'filePath',
            name: 'filePath',
            message: 'Vyber cestu k souboru',
            basePath: '.'
        },
    ])
    .then(async (answers) => {
        const { filePath } = answers;

        return filePath
    });

export default getPath;