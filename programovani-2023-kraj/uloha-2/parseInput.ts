const parseInput = (str: string): number | null => {
    str = str.replaceAll(" ", "");
    str = str.replaceAll(",", ".");
    try {
        if (!/\d{1,}\.?\d*\|?\d*/.test(str)) throw new Error('invalid')
        if (str.includes("|")) {
            return parseFloat(str.split('|')[0]) / parseFloat(str.split('|')[1])
        }
        return parseFloat(str)
    } catch (e) {
        console.error("Zadal jsi číslo v nesprávném formátu, zadej jako: {number}, nebo {number}|{number} pro zlomek")
        return null;
    }
}

export default parseInput;