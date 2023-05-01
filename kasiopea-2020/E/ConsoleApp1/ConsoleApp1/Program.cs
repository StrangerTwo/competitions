using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("E.txt"))
            {
                using (StreamWriter w = new StreamWriter("E-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {
                        int countOfSentences = Convert.ToInt32(r.ReadLine());
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');
                        int[] delkyVet = new int[tokens.Length];
                        int[] hodnoty = new int[tokens.Length];
                        bool[] skipujeme = new bool[tokens.Length];
                        for (int i = 0; i < delkyVet.Length; i++)
                        {
                            delkyVet[i] = Convert.ToInt32(tokens[i]);
                        }
                        for (int i = 0; i < delkyVet.Length; i++)
                        {
                            if (i == delkyVet.Length - 1)
                            {
                                hodnoty[i] = delkyVet[i - 1];
                            }
                            else if (i > 0)
                            {
                                int x = delkyVet[i - 1];
                                int y = delkyVet[i + 1];
                                hodnoty[i] = delkyVet[i - 1] + delkyVet[i + 1];
                            }
                        }

                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (delkyVet[i] >= hodnoty[i])
                            {
                                skipujeme[i] = true;
                            }
                        }

                        Console.Clear();
                        Console.WriteLine("Úkol : {0} z {1}", t+1, T);
                        Console.WriteLine("Vyřešeno : {0} z {1}", 0, tokens.Length);

                        int vyreseno = -1;
                        int cteniVRade = 0;
                        int prectenoVRade = 0;
                        int minSlov = 0;
                        for (int i = 0; i <= tokens.Length; i++)
                        {
                            if (i == tokens.Length || skipujeme[i] == true)
                            {
                                if (cteniVRade >= 3 || i == tokens.Length && cteniVRade >= 2)
                                {
                                    //je potreba optimalizovat
                                    int start = vyreseno + 2;
                                    int konec = i - 2;
                                    if (i == tokens.Length)
                                    {
                                        konec = i - 1;
                                    }
                                    int[] pole = new int[konec - start + 1];
                                    int nejlepsiKombinace = int.MaxValue;
                                    for (int j = 0; j < pole.Length; j++)
                                    {
                                        pole[j] = delkyVet[vyreseno + 2 + j];
                                    }
                                    if (konec - start + 1 > 50)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Úkol : {0} z {1}", t+1, T);
                                        Console.WriteLine("Vyřešeno : {0} z {1}", i, tokens.Length);
                                        Console.WriteLine("Počítám optimalizaci : {0} čísel", konec - start + 1);
                                        //for (int index = 0; index < pole.Length; index++)
                                        //{
                                        //    Console.Write(pole[index] + " ");
                                        //}
                                    }
                                    if (pole.Length == 1)
                                    {
                                        nejlepsiKombinace = 0;
                                    }
                                    else
                                    {
                                        int pocetNul = pocetNul = pole.Length / 2 + 1;
                                        if (pole.Length % 2 == 0)
                                        {
                                            pocetNul = pole.Length / 2;
                                        }
                                        for (int celkemNul = pocetNul; celkemNul >= pocetNul-3 && celkemNul > 0; celkemNul--)
                                        {
                                            int[] indexiNul = new int[celkemNul];
                                            //pole indexiNul je pokaždé seřazeno podle velikosti nejmenší => největší
                                            for (int index = 0; index < indexiNul.Length; index++)
                                            {
                                                indexiNul[index] = 0 + index * 2;
                                            }
                                            bool finished = false;
                                            do
                                            {
                                                // nastavit nulaZde[]
                                                bool[] nulaZde = new bool[pole.Length];
                                                foreach (int index in indexiNul)
                                                {
                                                    if (nulaZde[index] == true)
                                                    {
                                                        continue;
                                                    }
                                                    if (index == 0)
                                                    {
                                                        if (nulaZde[index + 1] == false)
                                                        {
                                                            nulaZde[index] = true;
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                    else if (index == nulaZde.Length-1)
                                                    {
                                                        if (nulaZde[index - 1] == false)
                                                        {
                                                            nulaZde[index] = true;
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (nulaZde[index - 1] == false && nulaZde[index + 1] == false)
                                                        {
                                                            nulaZde[index] = true;
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }

                                                // zjištění hodnoty kombinace
                                                int aktualniKombinace = 0;
                                                for (int j = 0; j < pole.Length; j++)
                                                {
                                                    if (nulaZde[j] == false)
                                                    {
                                                        aktualniKombinace += pole[j];
                                                    }
                                                }
                                                if (aktualniKombinace < nejlepsiKombinace)
                                                {
                                                    nejlepsiKombinace = aktualniKombinace;
                                                }

                                                // přičtení indexu na poslední místo
                                                bool pricteno = false;
                                                int poziceProPricteni = indexiNul.Length - 1;
                                                do
                                                {
                                                    if (finished)
                                                    {
                                                        break;
                                                    }
                                                    if (indexiNul[poziceProPricteni] < pole.Length - 1)
                                                    {
                                                        indexiNul[poziceProPricteni]++;
                                                        pricteno = true;
                                                        for (int index = 1; poziceProPricteni + index < indexiNul.Length; index++)
                                                        {
                                                            indexiNul[poziceProPricteni + index] = indexiNul[poziceProPricteni] + index * 2;
                                                            if (indexiNul[poziceProPricteni + index] == pole.Length)
                                                            {
                                                                pricteno = false;
                                                                poziceProPricteni--;
                                                                if (poziceProPricteni == -1)
                                                                {
                                                                    finished = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        poziceProPricteni--;
                                                        if (poziceProPricteni == -1)
                                                        {
                                                            finished = true;
                                                        }
                                                    }
                                                } while (!pricteno);

                                                // hotovo pokud jakýkoli číslo je větší než maxIndex
                                                foreach (int index in indexiNul)
                                                {
                                                    if (index >= pole.Length)
                                                    {
                                                        finished = true;
                                                    }
                                                }
                                            } while (!finished);
                                        }
                                        
                                    }

                                    int soucetNeoptimalizovany = delkyVet[vyreseno+1];
                                    if (i != tokens.Length)
                                    {
                                        soucetNeoptimalizovany += delkyVet[i-1];
                                    }
                                    vyreseno = i;
                                    minSlov += nejlepsiKombinace;
                                    minSlov += soucetNeoptimalizovany;
                                    cteniVRade = 0;
                                    prectenoVRade = 0;
                                }
                                else
                                {
                                    minSlov += prectenoVRade;
                                    cteniVRade = 0;
                                    prectenoVRade = 0;
                                    vyreseno = i;
                                }
                            }
                            else if (skipujeme[i] == false)
                            {
                                cteniVRade++;
                                prectenoVRade += delkyVet[i];
                            }
                        }
                        w.WriteLine(minSlov);
                    }
                }
            }
        }
    }
}