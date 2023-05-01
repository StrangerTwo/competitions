using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("D.txt"))
            {
                using (StreamWriter w = new StreamWriter("D-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        int[] values = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                        int R = values[0];
                        int S = values[1];
                        int K = values[2];

                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        char[,] map = new char[R, S];
                        for (int i = 0; i < R; i++)
                        {
                            int j = 0;
                            foreach (var item in r.ReadLine().ToCharArray())
                            {
                                map[i, j++] = item;
                            }
                        }

                        w.WriteLine(GetResult(map, R, S, K));
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static int GetResult(char[,] map, int R, int S, int K)
        {
            Point dvere = getDvereLocation(map, R, S);

            int pocetKroku = 0;
            int i = 1;
            foreach (var distance in PutDownBox(map, R, S, dvere).Take(K).ToArray())
            {
                if (distance == -1)
                {
                    return -1;
                }
                pocetKroku += distance;
                if (i != K)
                {
                    pocetKroku += distance;
                }
            }
            return pocetKroku;
        }

        private static IEnumerable<int> PutDownBox(char[,] map, int R, int S, Point dvere)
        {
            yield return 0;

            int maxDistanceAway = 1;
            int[,] placeDistance = new int[R, S];
            bool canAdvance = true;
            while (canAdvance)
            {
                canAdvance = false;
                for (int i = -maxDistanceAway; i <= maxDistanceAway; i++)
                {
                    for (int j = -(maxDistanceAway-Math.Abs(i)); j <= maxDistanceAway - Math.Abs(i); j++)
                    {
                        int x = dvere.X + i;
                        int y = dvere.Y + j;
                        if (ValidSpot(R, S, x, y))
                            if (map[x, y] != 'X')
                                if (placeDistance[x, y] == 0)
                                {
                                    int distanceFromDoor = CanReachSpot(placeDistance, R, S, x, y, dvere);
                                    if (distanceFromDoor != -1 && distanceFromDoor <= maxDistanceAway)
                                    {
                                        placeDistance[x, y] = distanceFromDoor;
                                        canAdvance = true;
                                        yield return distanceFromDoor;
                                    }
                                }
                    }
                }
                maxDistanceAway++;
            }
            while (true)
            {
                yield return -1;
            }
        }

        private static int CanReachSpot(int[,] placeDistance, int r, int s, int i, int j, Point dvere)
        {
            if (i == dvere.X && j == dvere.Y)
            {
                return -1;
            }
            int value = -1;

            if (ValidSpot(r, s, i - 1, j))
            {
                if (placeDistance[i - 1, j] > 0 || (i - 1 == dvere.X && j == dvere.Y))
                {
                    value = placeDistance[i - 1, j] + 1;
                }
            }
            if (ValidSpot(r, s, i + 1, j))
            {
                if (placeDistance[i + 1, j] > 0 || (i + 1 == dvere.X && j == dvere.Y))
                {
                    value = value == -1 ? placeDistance[i + 1, j] + 1 : Math.Min(value, placeDistance[i + 1, j] + 1);
                }
            }
            if (ValidSpot(r, s, i, j - 1))
            {
                if (placeDistance[i, j - 1] > 0 || (i == dvere.X && j - 1 == dvere.Y))
                {
                    value = value == -1 ? placeDistance[i, j - 1] + 1 : Math.Min(value, placeDistance[i, j - 1] + 1);
                }
            }
            if (ValidSpot(r, s, i, j + 1))
            {
                if (placeDistance[i, j + 1] > 0 || (i == dvere.X && j + 1 == dvere.Y))
                {
                    value = value == -1 ? placeDistance[i, j + 1] + 1 : Math.Min(value, placeDistance[i, j + 1] + 1);
                }
            }
            return value;
        }

        private static bool ValidSpot(int r, int s, int i, int j)
        {
            return i > -1 && j > -1 && i < r && j < s;
        }

        private static Point getDvereLocation(char[,] map, int R, int S)
        {
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < S; j++)
                {
                    if (map[i, j] == 'D')
                    {
                        return new Point(i, j);
                    }
                }
            }
            return new Point(0, 0);
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
