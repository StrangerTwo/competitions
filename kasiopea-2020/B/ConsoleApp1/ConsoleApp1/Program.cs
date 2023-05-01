using System;
using System.Collections.Generic;
using System.IO;

namespace A
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("B.txt"))
            {
                using (StreamWriter w = new StreamWriter("B-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {
                        
                        int countOfNumbers = Convert.ToInt32(r.ReadLine());
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');
                        int maximum;
                        int nejmensiMaximum = int.MaxValue;

                        List<int> lastThree = new List<int>();

                        for (int i = 0; i < countOfNumbers; i++)
                        {
                            lastThree.Add(Convert.ToInt32(tokens[i]));
                            if (i >= 3)
                            {
                                lastThree.RemoveAt(0);
                            }
                            if (i >=2)
                            {
                                maximum = int.MinValue;
                                foreach (int item in lastThree)
                                {
                                    if (item > maximum)
                                    {
                                        maximum = item;
                                    }
                                }
                                if (maximum < nejmensiMaximum)
                                {
                                    nejmensiMaximum = maximum;
                                }
                            }
                            
                        }

                        

                        w.WriteLine(nejmensiMaximum);
                    }
                }
            }
        }
    }
}