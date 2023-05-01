using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace oprava_levopisu
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Vyber soubor slovníku.");
            Console.ReadKey();


            List<string> slovnik = new List<string>();
            saveSlovnik(ref slovnik);
            if (slovnik.Count == 0)
            {
                return;
            }

            Console.WriteLine("Vyber soubor Slohomilova textu.");
            Console.ReadKey();

            List<string> levopis = new List<string>();
            saveLevopis(ref levopis);
            if (levopis.Count == 0)
            {
                return;
            }

            // Musíme procházet slova a hledat k nim jejich správné názvy

            List<string> oprava = new List<string>();
            opravLevopis(slovnik, levopis, ref oprava);

            //Vytvoříme soubor opravy.txt

            Console.WriteLine("Vyber soubor kam mám uložit opravu.");
            Console.ReadKey();

            saveOprava(oprava);

            Console.WriteLine("Levopis byl opraven!");

            Console.ReadKey();
        }

        static void opravLevopis(List<string> slovnik, List<string> levopis, ref List<string> oprava)
        {
            Console.WriteLine("Opravuji levopis..");
            foreach (string slovo in levopis)
            {
                List<string> seznamOprav = new List<string>();
                //musíme najít opravu

                seznamOprav.AddRange(opravChybiciZnak(slovnik.Where(x => (x.Length - slovo.Length) == 1).ToList(), slovo));
                seznamOprav.AddRange(opravZnakNavic(slovnik.Where(x => (x.Length - slovo.Length) == -1).ToList(), slovo));
                seznamOprav.AddRange(opravJinyZnak(slovnik.Where(x => x.Length == slovo.Length).ToList(), slovo));

                oprava.Add(String.Join(" ", seznamOprav.OrderBy((x) => slovnik.IndexOf(x)).ToArray()));
            }
            Console.WriteLine("OK!");
        }

        static List<string> opravChybiciZnak(List<string> slovnik, string slovo)
        {
            List<string> opravenaSlova = new List<string>();

            foreach (string slovnikoveSlovo in slovnik)
            {
                int chyba = 0;
                for (int i = 0; i < slovnikoveSlovo.Length; i++)
                {
                    if (i - chyba == slovo.Length || slovo[i - chyba] != slovnikoveSlovo[i])
                    {
                        chyba++;
                        if (chyba == 2)
                        {
                            break;
                        }
                    }
                }
                if (chyba == 1)
                {
                    opravenaSlova.Add(slovnikoveSlovo);
                }
            }

            return opravenaSlova;
        }

        static List<string> opravZnakNavic(List<string> slovnik, string slovo)
        {
            List<string> opravenaSlova = new List<string>();

            foreach (string slovnikoveSlovo in slovnik)
            {
                int chyba = 0;
                for (int i = 0; i < slovo.Length; i++)
                {
                    if (i - chyba == slovnikoveSlovo.Length ||  slovo[i] != slovnikoveSlovo[i - chyba])
                    {
                        chyba++;
                        if (chyba == 2)
                        {
                            break;
                        }
                    }
                }
                if (chyba == 1)
                {
                    opravenaSlova.Add(slovnikoveSlovo);
                }
            }

            return opravenaSlova;
        }

        static List<string> opravJinyZnak(List<string> slovnik, string slovo)
        {
            List<string> opravenaSlova = new List<string>();

            foreach (string slovnikoveSlovo in slovnik)
            {
                int chyba = 0;
                for (int i = 0; i < slovo.Length; i++)
                {
                    if (slovo[i] != slovnikoveSlovo[i])
                    {
                        chyba++;
                        if (chyba == 2)
                        {
                            break;
                        }
                    }
                }
                if (chyba <= 1)
                {
                    opravenaSlova.Add(slovnikoveSlovo);
                }
            }

            return opravenaSlova;
        }

        static void saveOprava(List<string> oprava)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.InitialDirectory = "D:\\Programovani\\Uceni\\Zadání\\soutezici\\programovani\\otrava-levopisu";
            sFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sFD.FileName = "oprava.txt";

            if (sFD.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Console.WriteLine("Ukládám opravu..");

            using (StreamWriter writer = new StreamWriter(sFD.FileName))
            {
                foreach (string slovo in oprava)
                {
                    writer.WriteLine(slovo);
                }
            }

            Console.WriteLine("OK!");
        }

        static void saveSlovnik(ref List<string> slovnik)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.InitialDirectory = "D:\\Programovani\\Uceni\\Zadání\\soutezici\\programovani\\otrava-levopisu";
            oFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (oFD.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Console.WriteLine("Načítám slovník..");

            string line;
            using (StreamReader reader = new StreamReader(oFD.FileName))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    slovnik.Add(line);
                }
            }
            
            Console.WriteLine("OK!");
            Console.WriteLine("Počet slov ve slovníku :" + slovnik.Count);
        }

        static void saveLevopis(ref List<string> levopis)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.InitialDirectory = "D:\\Programovani\\Uceni\\Zadání\\soutezici\\programovani\\otrava-levopisu";
            oFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (oFD.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Console.WriteLine("Načítám text..");

            string line;
            using (StreamReader reader = new StreamReader(oFD.FileName))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    levopis.Add(line);
                }
            }
            
            Console.WriteLine("OK!");
            Console.WriteLine("Počet slov v levopisu :" + levopis.Count);
        }
    }
}
