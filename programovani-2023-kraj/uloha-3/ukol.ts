import fs from 'fs'
import path from "path"
import getFolder from './getFolder.js'
import parseImg from './parseImg.js'

const ukol =  async () => {

    const folderPath = await getFolder()
    
    const imagePaths = fs.readdirSync(folderPath).map(x => path.relative(".", folderPath + "/" + x))
    
    if (imagePaths.length !== 4) {
      console.error("Ve složce se nenachází 4 obrázky, prosím vyberte znovu.");
      return
    }

    console.log(imagePaths)

    const images = await parseImg(imagePaths);

    console.log('Obrázky načteny')
}

export default ukol;