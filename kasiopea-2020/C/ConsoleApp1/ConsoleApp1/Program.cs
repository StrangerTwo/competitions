using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("C.txt"))
            {
                using (StreamWriter w = new StreamWriter("C-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {

                        int countOfNumbers = Convert.ToInt32(r.ReadLine());
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');
                        double maximalniRozdil = 0;
                        double minimalniSvetelnost;
                        string minimalniSvetelnostStr;

                        List<int> lampArray = new List<int>();
                        
                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            lampArray.Add(Convert.ToInt32(tokens[i]));
                        }
                        lampArray.Sort();
                        for (int i = 1; i < countOfNumbers; i++)
                        {
                            if (lampArray[i] - lampArray[i-1] > maximalniRozdil)
                            {
                                maximalniRozdil = lampArray[i] - lampArray[i - 1];
                            }
                        }
                        minimalniSvetelnost = maximalniRozdil / 2;
                        minimalniSvetelnostStr = Convert.ToString(minimalniSvetelnost);
                        if (minimalniSvetelnostStr.Contains(","))
                        {
                            minimalniSvetelnostStr = minimalniSvetelnostStr.Split(',')[0] + "." + minimalniSvetelnostStr.Split(',')[1];
                        }

                        w.WriteLine(minimalniSvetelnostStr);
                    }
                }
            }
        }
    }
}