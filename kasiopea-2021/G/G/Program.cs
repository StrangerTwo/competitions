using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("G.txt"))
            {
                using (StreamWriter w = new StreamWriter("G-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        var values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int N = values[0];
                        int M = values[1];

                        Dictionary<int, List<int>> knownWins = new Dictionary<int, List<int>>();

                        for (int i = 0; i < M; i++)
                        {
                            values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

                            if (!knownWins.ContainsKey(values[0]))
                            {
                                knownWins.Add(values[0], new List<int>());
                            }
                            knownWins[values[0]].Add(values[1]);
                        }

                        w.WriteLine(GetResult(N, knownWins).ToStr());
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static List<int> GetResult(int N, Dictionary<int, List<int>> victories)
        {
            List<int> order = new List<int>();

            if (!victories.ContainsKey(1))
            {
                List<int> fail = new List<int>();
                fail.Add(-1);
                return fail;
            }

            List<int> currentIndexes = new List<int>();
            currentIndexes.Add(0);
            while (true)
            {
                int thisCompetitor = getCompetitor(victories, currentIndexes);
                if (thisCompetitor == -1)
                {
                    currentIndexes.RemoveAt(currentIndexes.Count - 1);
                    if (currentIndexes.Count == 0)
                    {
                        break;
                    }
                    currentIndexes[currentIndexes.Count - 1]++;
                    continue;
                }

                if (!order.Contains(thisCompetitor))
                {
                    order.Add(thisCompetitor);
                    if (victories.ContainsKey(thisCompetitor))
                    {
                        currentIndexes.Add(0);
                    }
                    else
                    {
                        currentIndexes[currentIndexes.Count - 1]++;
                    }
                }
                else
                {
                    currentIndexes[currentIndexes.Count - 1]++;
                }
            }

            //AddVictories(order, victories, 1);

            if (order.Count < N)
            {
                List<int> fail = new List<int>();
                fail.Add(-1);
                return fail;
            }

            order.Reverse();
            return order;
        }

        private static int getCompetitor(Dictionary<int, List<int>> victories, List<int> currentIndexes)
        {
            if (currentIndexes.Count == 1 && currentIndexes[0] == 1)
            {
                return -1;
            }
            int competitor = 1;
            for (int i = 1; i < currentIndexes.Count; i++)
            {
                int index = currentIndexes[i];
                if (index < victories[competitor].Count)
                {
                    competitor = victories[competitor][index];
                }
                else
                {
                    return -1;
                }
            }
            return competitor;
        }

        private static void AddVictories(List<int> order, Dictionary<int, List<int>> victories, int competitor)
        {
            if (order.Contains(competitor))
            {
                return;
            }
            order.Add(competitor);

            if (victories.ContainsKey(competitor))
            {
                foreach (var loser in victories[competitor])
                {
                    AddVictories(order, victories, loser);
                }
            }
        }
    }

    public static class Extensions
    {
        public static string ToStr<T>(this IEnumerable<T> collection)
        {
            string str = "";
            foreach (var item in collection)
            {
                str += item + " ";
            }

            return str.Remove(str.Length - 1);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
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
