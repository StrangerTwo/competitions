using System;
using System.IO;

namespace A
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("A.txt"))
            {
                using (StreamWriter w = new StreamWriter("A-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');

                        int ziskSReklamou = int.Parse(tokens[0]);
                        int ziskBezReklamy = int.Parse(tokens[1]);
                        int cenaReklamy = int.Parse(tokens[2]);

                        ///////////////////////
                        // SEM DOPLŇ SVŮJ KÓD
                        ///////////////////////
                        // můžeš využít proměnné ziskSReklamou, ziskBezReklamy, cenaReklamy
                        // výsledek ulož do proměnné výsledek, např.:

                        string vysledek;
                        if (ziskSReklamou - cenaReklamy > ziskBezReklamy)
                        {
                            vysledek = "REKLAMU";
                        }
                        else
                        {
                            vysledek = "NE REKLAMU";
                        }


                        w.WriteLine(vysledek);
                    }
                }
            }
        }
    }
}