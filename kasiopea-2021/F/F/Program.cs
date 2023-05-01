using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("F.txt"))
            {
                using (StreamWriter w = new StreamWriter("F-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        var values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int N = values[0];
                        int M = values[1];

                        Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>();

                        for (int i = 0; i < M; i++)
                        {
                            values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

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

                        GetResult(w, N, paths);
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static void GetResult(StreamWriter w, int n, Dictionary<int, List<int>> paths)
        {
            bool[] visited = new bool[n];

            int start = 1;
            int sum = 0;


            Stack<int> stack = new Stack<int>();
            GoThrough(stack, paths, visited, start, ref sum, 0);

            while (stack.Count > 0)
            {
                int x = stack.Pop();
                GoThrough(stack, paths, visited, x, ref sum, 5);
            }

            w.WriteLine(sum);
        }

        private static void GoThrough(Stack<int> stack, Dictionary<int, List<int>> paths, bool[] visited, int x, ref int sum, int num)
        {
            if (visited[x - 1]) return;       // this should never happen

            visited[x - 1] = true;
            sum += num;

            foreach (var item in paths[x].Where(p => !visited[p - 1]).Skip(1))
            {
                stack.Push(item);
            }

            int position = paths[x].Where(p => !visited[p - 1]).FirstOrDefault();
            if (position != 0)
            {
                GoThrough(stack, paths, visited, position, ref sum, 3);
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
    }
}
