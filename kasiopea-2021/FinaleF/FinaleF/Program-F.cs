using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleF
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
                        int M = data[1];
                        int K = data[2];

                        int[] ratings = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();


                        w.WriteLine(GetResult(N, M, K, ratings.OrderByDescending(x => x).ToList()));
                    }
                }
            }
        }

        private static string GetResult(int n, int m, int k, List<int> ratings)
        {
            if (k % m != 0 || m > k)
            {
                return "NE";
            }

            int countOfGoodRaters = ratings.Where(x => x < m).Count();
            if (countOfGoodRaters * m == k)  
            {
                return "ANO";
            }
            else if(ratings.Where(x => x > m).Count() > 0)
            {
                if (countOfGoodRaters + 1 * m == k)
                {
                    return "ANO";
                }
            }

            return "NE";
        }
    }
}
