using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaleA
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("A.txt"))
            {
                using (StreamWriter w = new StreamWriter("A-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());
                        int K = Convert.ToInt32(r.ReadLine());

                        int[] sizes = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();


                        w.WriteLine(GetResult(N, K, sizes));
                    }
                }
            }
        }

        private static int GetResult(int n, int k, int[] sizes)
        {
            int count = 0;
            for (int i = 0; i < k; i++)
            {
                count += Math.Min(sizes[i], n);
            }
            return count;
        }
    }
}
