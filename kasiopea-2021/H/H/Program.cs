using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("H.txt"))
            {
                using (StreamWriter w = new StreamWriter("H-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        var values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int R = values[0];
                        int S = values[1];

                        int[,] tables = new int[R, S];

                        for (int i = 0; i < R; i++)
                        {
                            int j = 0;
                            foreach (var number in r.ReadLine().ToCharArray().Select(x => (int)Char.GetNumericValue(x)))
                            {
                                tables[i, j++] = number;
                            }
                        }


                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        GetResult(w, tables, R, S);
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static void GetResult(StreamWriter w, int[,] arr, int r, int s)
        {
            List<int[,]> groups = new List<int[,]>();
            groups.Add(new int[r, s]);
            groups.Add(new int[r, s]);
            groups.Add(new int[r, s]);

            List<int>[] lastRowConnections = new List<int>[3];

            for (int i = 0; i < lastRowConnections.Length; i++)
            {
                lastRowConnections[i] = new List<int>();
            }

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    switch (arr[i, j])
                    {
                        case 1:
                            // Two options: 
                                // You HAVE To from previous row
                                // Get in default
                            break;
                        case 2:
                            // Options:
                                // 
                            break;
                    }
                }
            }

            foreach (var group in groups)
            {
                foreach (var row in group.GetRows())
                {
                    w.WriteLine(row.ToStrRow());
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

        public static T[] SliceRow<T>(this T[,] arr, int row)
        {
            T[] result = new T[arr.GetLength(1)];
            if (row < 0 || row >= arr.GetLength(0))
            {
                return result;
            }
            for (var i = 0; i < arr.GetLength(1); i++)
            {
                result[i] = arr[row, i];
            }
            return result;
        }

        public static IEnumerable<T[]> GetRows<T>(this T[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                yield return array.SliceRow(i);
            }
        }

        public static string ToStrRow<T>(this IEnumerable<T> arr)
        {
            string str = "";
            foreach (var item in arr)
            {
                str += item;
            }
            return str;
        }
    }
}
