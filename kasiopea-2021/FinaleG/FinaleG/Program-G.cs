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
            using (StreamReader r = new StreamReader("F.txt"))
            {
                using (StreamWriter w = new StreamWriter("F-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int[] data = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int N = data[0];
                        int K = data[1];

                        char[] animals = r.ReadLine().ToCharArray();


                        w.WriteLine(GetResult(N, K, animals));
                    }
                }
            }
        }

        private static string GetResult(int n, int k, char[] animals)
        {
            for (int i = 0; i < k; i++)
            {
                animals = ChangeAnimals(animals, n);
            }
            return string.Join("", animals);
        }

        private static char[] ChangeAnimals(char[] lastDay, int n)
        {
            char[] newDay = lastDay.ToArray();

            for (int j = 0; j < n; j++)
            {
                int left = j - 1 < 0 ? n - 1 : j - 1;
                int right = j + 1 == n ? 0 : j + 1;
                newDay[j] = ChangeAnimal(lastDay[j], lastDay[left], lastDay[right]);
            }
            return newDay;
        }

        private static char ChangeAnimal(char v1, char v2, char v3)
        {
            if (v1 == 'X')
            {
                if (v2 == 'Y' && v3 == 'X')
                {
                    return 'Y';
                }
                else if (v2 == 'X' && v3 == 'Y')
                {
                    return 'Y';
                }
                return 'X';
            }
            else
            {
                if (v2 == 'Y' && v3 == 'Y')
                {
                    return 'X';
                }
                else if (v2 == 'X' && v3 == 'X')
                {
                    return 'X';
                }
                return 'Y';
            }
        }
    }
}
