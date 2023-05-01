using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleG
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("G.txt"))
            {
                using (StreamWriter w = new StreamWriter("G-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        long[] data = r.ReadLine().Split(' ').Select(x => Convert.ToInt64(x)).ToArray();
                        long N = data[0];
                        long K = data[1];

                        char[] animals = r.ReadLine().ToCharArray();


                        w.WriteLine(GetResult(N, K, animals));
                    }
                }
            }
        }

        private static string GetResult(long n, long k, char[] animals)
        {
            for (int i = 0; i < k; i++)
            {
                animals = ChangeAnimals(animals, n);
                int y = animals.Where(a => a == 'Y').Count();

                if (y == 0)
                {
                    return string.Join("", animals);
                }
                if (k > 10)
                {
                    if (i % (k / 10) == 0)
                    {
                        Console.Write(".");
                    }
                }
            }
            if (k > 10) Console.WriteLine();
            return string.Join("", animals);
        }

        private static char[] ChangeAnimals(char[] lastDay, long n)
        {
            char[] newDay = lastDay.ToArray();

            for (int j = 0; j < n; j++)
            {
                long left = j - 1 < 0 ? n - 1 : j - 1;
                long right = j + 1 == n ? 0 : j + 1;
                newDay[j] = ChangeAnimal(lastDay[j], lastDay[left], lastDay[right]);
            }
            return newDay;
        }

        private static char ChangeAnimal(char v1, char v2, char v3)
        {
            return (int)v2 - (int)v3 == 0 ? 'X' : 'Y';
        }
    }
}
