using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("A.txt"))
            {
                using (StreamWriter w = new StreamWriter("A-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int i = 1; i <= T; i++)
                    {
                        int c = Convert.ToInt32(r.ReadLine());
                        int[] a = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int[] values = new int[] { 1, 2, 5, 10, 20, 50 };

                        int total = 0;
                        a.forEach((x, index) => total += values[index] * x);

                        if (total >= c)
                        {
                            w.WriteLine("ANO");
                        }
                        else
                        {
                            w.WriteLine("NE");
                        }
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }
    }

    public static class Extensions
    {
        public static void forEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in collection)
            {
                action(item, i++);
            }
        }
    }
}
