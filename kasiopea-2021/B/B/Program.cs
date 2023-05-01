using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("B.txt"))
            {
                using (StreamWriter w = new StreamWriter("B-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int i = 1; i <= T; i++)
                    {
                        int N = Convert.ToInt32(r.ReadLine());
                        int[] a = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

                        int twoCount = 0;
                        int threeCount = 0;

                        a.forEach(x =>
                        {
                            if (x % 2 == 0)
                            {
                                twoCount++;
                            }
                            else
                            {
                                threeCount++;
                            }
                        });

                        if (twoCount > threeCount)
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
        public static void forEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

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
