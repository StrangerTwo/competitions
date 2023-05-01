using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleB
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("B.txt"))
            {
                using (StreamWriter w = new StreamWriter("B-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());


                        w.WriteLine(GetResult(N));
                    }
                }
            }
        }

        private static string GetResult(int n)
        {
            if (n < 4 || n % 2 == 1)
            {
                return "-1";
            }

            string str = "";
            if (n % 4 == 0)
            {
                for (int i = 1; i <= n; i += 4)
                {
                    str += $"{i} {i + 1}\n";
                    str += $"{i} {i + 2}\n";
                    str += $"{i} {i + 3}\n";
                    str += $"{i + 1} {i + 2}\n";
                    str += $"{i + 1} {i + 3}\n";
                    str += $"{i + 2} {i + 3}\n";
                }
            }
            else
            {
                for (int i = 1; i <= n - 6; i += 4)
                {
                    str += $"{i} {i + 1}\n";
                    str += $"{i} {i + 2}\n";
                    str += $"{i} {i + 3}\n";
                    str += $"{i + 1} {i + 2}\n";
                    str += $"{i + 1} {i + 3}\n";
                    str += $"{i + 2} {i + 3}\n";
                }
                int x = n - 5;
                // Kruh
                str += $"{x} {x + 1}\n";
                str += $"{x + 1} {x + 2}\n";
                str += $"{x + 2} {x + 3}\n";
                str += $"{x + 3} {x + 4}\n";
                str += $"{x + 4} {x + 5}\n";
                str += $"{x + 5} {x}\n";

                // 3 úhlopříčky
                str += $"{x} {x + 3}\n";
                str += $"{x + 1} {x + 4}\n";
                str += $"{x + 2} {x + 5}\n";
            }
            return str;
        }
    }
}
