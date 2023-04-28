import parseInput from './parseInput.js';
const inquirer = (await import('inquirer')).default

const ukol = async () => {

    // zadani
    const numberA = await inquirer
    .prompt([
    {
        type: 'input',
        name: 'numberA',
        message: 'Zadej číslo A',
    },
    ])
    .then(answers => answers.numberA);

    // validace
    const numA = parseInput(numberA);
    if (numA === null) return;

    //  zadani
    const operand = await inquirer
        .prompt([
        {
            type: 'list',
            name: 'operand',
            message: 'Vyber znaménko',
            choices: [
            "+",
            "-",
            "*",
            "/"
            ]
        },
        ])
        .then(answers => answers.operand);

    const numberB = await inquirer
    .prompt([
    {
        type: 'input',
        name: 'numberB',
        message: 'Zadej číslo B',
    },
    ])
    .then(answers => answers.numberB);

    const numB= parseInput(numberB);
    if (numB === null) return;

    switch (operand) {
    case "+":
        console.log(`Výsledek ${numberA} + ${numberB} = ${numA + numB}`)
        break;
    case "-":
        console.log(`Výsledek ${numberA} - ${numberB} = ${numA - numB}`)
        break;
    case "/":
        console.log(`Výsledek ${numberA} / ${numberB} = ${numA / numB}`)
        break;
    case "*":
        console.log(`Výsledek ${numberA} * ${numberB} = ${numA * numB}`)
        break;
    }
}

export default ukol;