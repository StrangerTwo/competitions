using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("F.txt"))
            {
                using (StreamWriter w = new StreamWriter("F-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {
                        int listCount = Convert.ToInt32(r.ReadLine());
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');
                        int[] listValues = new int[tokens.Length];
                        bool muzeExistovat = true;
                        bool existuje = false;
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            listValues[i] = Convert.ToInt32(tokens[i]);
                        }

                        //VŠECHNY STEJNÉ - SOUČET NEDĚLITELNÝ
                        bool vsechnyStejne = true;
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            if (listValues[i] != listValues[0])
                            {
                                vsechnyStejne = false;
                            }
                        }
                        if (vsechnyStejne)
                        {
                            if (listCount % listValues[0] != 0)
                            {
                                muzeExistovat = false;
                            }
                            else
                            {
                                existuje = true;
                            }
                        }


                        //VŠECHNY SUDÉ - SOUČET LICHÝ
                        bool vsechnySude = true;
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            if (listValues[i] % 2 == 0)
                            {
                                vsechnySude = false;
                            }
                        }
                        if (vsechnySude && listCount % 2 == 1)
                        {
                            muzeExistovat = false;
                        }



                        int[] odkazy = new int[listValues.Length];
                        for (int i = 0; i < odkazy.Length; i++)
                        {
                            odkazy[i] = -1;
                        }
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            int adresa = i + listValues[i];
                            if (adresa >= listValues.Length)
                            {
                                adresa -= listValues.Length;
                            }
                            if (odkazy[adresa] == -1)
                            {
                                odkazy[adresa] = i;
                            }
                        }

                        List<int> bigNumbers = new List<int>();
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            if (listValues[i] > 0.2*listCount)
                            {
                                bigNumbers.Add(i);
                            }
                        }

                        bool[] finished = new bool[listValues.Length];
                        int[] finishCount = new int[listValues.Length];
                        int[] finishIndex = new int[listValues.Length];
                        if (muzeExistovat && existuje == false)
                        {
                            //brute force hledani reseni
                            for (int i = 0; i < bigNumbers.Count; i++)
                            {
                                int index = bigNumbers[i];
                                int totalCount = 0;
                                if (odkazy[bigNumbers[i]] != -1)
                                {
                                    if (finishCount[odkazy[bigNumbers[i]]] != 0)
                                    {
                                        index = finishIndex[odkazy[bigNumbers[i]]];
                                        totalCount = finishCount[odkazy[bigNumbers[i]]] - listValues[odkazy[bigNumbers[i]]];
                                    }
                                }

                                do
                                {
                                    totalCount += listValues[index];
                                    index += listValues[index];
                                    if (index >= listValues.Length)
                                    {
                                        index -= listValues.Length;
                                    }
                                    if (totalCount == listValues.Length)
                                    {
                                        existuje = true;
                                    }
                                } while (totalCount < listValues.Length);
                                finishCount[bigNumbers[i]] = totalCount;
                                finishIndex[bigNumbers[i]] = index;
                                finished[bigNumbers[i]] = true;
                                if (existuje)
                                {
                                    break;
                                }
                            }
                        }

                        Console.Clear();
                        Console.WriteLine("Počítám úkol {0} z {1}",t+1,T);
                        Console.WriteLine("celkem lístků : {0}",listCount);
                        if (muzeExistovat && existuje == false)
                        {
                            //brute force hledani reseni
                            for (int i = 0; i < listValues.Length; i++)
                            {
                                if (finished[i])
                                {
                                    continue;
                                }
                                int index = i;
                                int totalCount = 0;
                                if (odkazy[i] != -1)
                                {
                                    if (finishCount[odkazy[i]] != 0)
                                    {
                                        index = finishIndex[odkazy[i]];
                                        totalCount = finishCount[odkazy[i]] - listValues[odkazy[i]];
                                    }
                                }

                                do
                                {
                                    totalCount += listValues[index];
                                    index += listValues[index];
                                    if (index >= listValues.Length)
                                    {
                                        index -= listValues.Length;
                                    }
                                    if (totalCount == listValues.Length)
                                    {
                                        existuje = true;
                                    }
                                } while (totalCount < listValues.Length);
                                finishCount[i] = totalCount;
                                finishIndex[i] = index;
                                if (existuje)
                                {
                                    break;
                                }
                            }
                        }

                        if (existuje)
                        {
                            w.WriteLine("ANO");
                        }
                        else
                        {
                            w.WriteLine("NE");
                        }
                    }
                }
            }
        }
    }
}