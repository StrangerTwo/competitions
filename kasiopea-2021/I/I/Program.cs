using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("I.txt"))
            {
                using (StreamWriter w = new StreamWriter("I-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        int N = Convert.ToInt32(r.ReadLine());

                        Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>(N);
                        for (int i = 0; i < N - 1; i++)
                        {
                            var values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

                            if (!paths.ContainsKey(values[0]))
                            {
                                paths.Add(values[0], new List<int>());
                            }
                            paths[values[0]].Add(values[1]);

                            if (!paths.ContainsKey(values[1]))
                            {
                                paths.Add(values[1], new List<int>());
                            }
                            paths[values[1]].Add(values[0]);
                        }


                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        GetResult(w, N, paths);
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static void GetResult(StreamWriter w, int n, Dictionary<int, List<int>> paths)
        {
            var maxTiers = paths.Select(x => {
                // get sum of its children
                int maxTier = 0;

                Dictionary<int, int> distanceAway = new Dictionary<int, int>();
                Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
                stack.Push(new Tuple<int, int>(x.Key, 1));

                while (stack.Count > 0)
                {
                    if (maxTier > 20)
                    {
                        break;
                    }

                    var y = stack.Pop();


                    if (distanceAway.ContainsKey(y.Item1)) continue;
                    distanceAway.Add(y.Item1, y.Item2);

                    maxTier = Math.Max(maxTier, y.Item2);
                    foreach (var item in paths[y.Item1])
                    {
                        stack.Push(new Tuple<int, int>(item, y.Item2 + 1));
                    }
                }

                return new Tuple<int, int, Dictionary<int, int>>(x.Key, maxTier, distanceAway);
            }).OrderBy(x => x.Item2).ToArray();

            var bestPosition = maxTiers.First();

            w.WriteLine(bestPosition.Item2);
            w.WriteLine(string.Join(" ", bestPosition.Item3.OrderBy(x => x.Key).Select(x => x.Value)));
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
    }
}
