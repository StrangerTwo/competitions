using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("C.txt"))
            {
                using (StreamWriter w = new StreamWriter("C-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        int[] values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int R = values[0];
                        int S = values[1];
                        int K = values[2];

                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int[,] cake = new int[R, S];
                        for (int i = 0; i < cake.GetLength(0); i++)
                        {
                            int j = 0;
                            foreach (var item in r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)))
                            {
                                cake[i, j++] = item;
                            }
                        }

                        w.WriteLine(MatrixToStr(GetResult(cake, K)));
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static string MatrixToStr(int[,] vs)
        {
            string result = "";
            foreach (var row in vs.GetRows())
            {
                result += row.ToStr() + "\n";
            }
            return result.Remove(result.Length - 1);
        }

        private static int[,] GetResult(int[,] cake, int K)
        {
            var arr = (int[,])cake.Clone();

            Point lastDrawn = new Point(-1, -1);
            int lastValue = -1;
            int tempValue = -1;

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i,j] != -1)
                    {
                        fillInRect(arr, lastDrawn, new Point(i, j), arr[i, j]);
                        lastDrawn = new Point(lastDrawn.X, j);
                        lastValue = tempValue = arr[i, j];
                    }
                }
                if (tempValue != -1)
                {
                    fillInRect(arr, lastDrawn, new Point(i, arr.GetLength(1) - 1), tempValue);      //Finish off the row
                    lastDrawn = new Point(i, -1);
                }
                tempValue = -1;
            }

            if (lastDrawn.X != arr.GetLength(0) - 1)
            {
                fillInRect(arr, lastDrawn, new Point(arr.GetLength(0) - 1, arr.GetLength(1) - 1), lastValue);
            }

            return arr;
        }

        private static void fillInRect(int[,] arr, Point lastDrawn, Point point, int value)
        {
            for (int i = lastDrawn.X + 1; i <= point.X; i++)
            {
                for (int j = lastDrawn.Y + 1; j <= point.Y; j++)
                {
                    arr[i, j] = value;
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

        public static IEnumerable<T> SliceRow<T>(this T[,] arr, int row)
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

        public static IEnumerable<IEnumerable<T>> GetRows<T>(this T[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                yield return array.SliceRow(i);
            }
        }

        public static string ToStr<T>(this IEnumerable<T> arr)
        {
            string str = "";
            bool first = true;
            foreach (var item in arr)
            {
                if (!first)
                    str += ' ';

                first = false;
                str += item;
            }
            return str;
        }
    }
}
