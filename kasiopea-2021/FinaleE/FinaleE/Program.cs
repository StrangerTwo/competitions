using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleE
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("E.txt"))
            {
                using (StreamWriter w = new StreamWriter("E-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        long[] data = r.ReadLine().Split(' ').Select(x => Convert.ToInt64(x)).ToArray();
                        long L = data[0];
                        long N = data[1];
                        long M = data[2];

                        long[] lentilky = r.ReadLine().Split(' ').Select(x => Convert.ToInt64(x)).ToArray();
                        long[] holes = r.ReadLine().Split(' ').Select(x => Convert.ToInt64(x)).ToArray();


                        w.WriteLine(GetResult(L,N,M, lentilky.OrderBy(x => x), holes.OrderBy(x => x).ToArray()));
                    }
                }
            }
        }

        private static long GetResult(long l, long n, long m, IEnumerable<long> lentilky, long[] holes)
        {
            long longestRightDistance = 0;

            int holeIndex = 0;
            foreach (var item in lentilky)
            {
                for (int i = holeIndex; i < holes.Length; i++)
                {
                    if (holes[i] < item)
                    {
                        continue;
                    }
                    holeIndex = i;
                    if (holes[i] - item > longestRightDistance)
                    {
                        longestRightDistance = holes[i] - item;
                        break;
                    }
                    break;
                }
            }

            return longestRightDistance;
        }
    }
}
