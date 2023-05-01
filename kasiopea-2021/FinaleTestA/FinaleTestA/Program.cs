using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaleTestA
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("A.txt"))
            {
                using (StreamWriter w = new StreamWriter("A-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());
                        int V = Convert.ToInt32(r.ReadLine());

                        List<int[]> towerCoords = new List<int[]>();
                        for (int i = 0; i < V; i++)
                        {
                            towerCoords.Add(r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray());
                        }


                        w.WriteLine(GetResult(N, V, towerCoords));
                    }
                }
            }
        }

        private static int GetResult(int n, int v, List<int[]> towerCoords)
        {
            Stack<Zone> zones = new Stack<Zone>();
            zones.Push(new Zone(0, 0, n - 1, n - 1));

            foreach (var item in towerCoords)
            {
                zones = getNewZones(zones, item);
            }

            if (zones.Count == 0)
            {
                return 0;
            }

            return zones.Select(x => x.Area()).Max();
        }

        private static Stack<Zone> getNewZones(Stack<Zone> lastZones, int[] item)
        {
            var zones = new Stack<Zone>();

            foreach (var lastZone in lastZones)
            {
                foreach (var zone in lastZone.newLocation(item[0], item[1]))
                {
                    zones.Push(zone);
                }
            }
            return zones;
        }
    }

    public struct Zone
    {
        int top;
        int left;
        int right;
        int bottom;

        public Zone(int top, int left, int bottom, int right)
        {
            this.top = top;
            this.left = left;
            this.right = right;
            this.bottom = bottom;
        }

        public int Area()
        {
            return (bottom - top + 1) * (right - left + 1);
        }

        public List<Zone> newLocation(int x, int y)
        {
            var zones = new List<Zone>();

            if (isWithin(x, top, bottom) && isWithin(y, left, right))
            {
                if (top < x && left < y)        // top left
                {
                    zones.Add(new Zone(top, left, x - 1, y - 1));
                }
                if (top < x && right > y)       // top right
                {
                    zones.Add(new Zone(top, y + 1, x - 1, right));
                }
                if (bottom > x && left < y)     // bottom left
                {
                    zones.Add(new Zone(x + 1, left, bottom, y - 1));
                }
                if (bottom > x && right > y)    // bottom right
                {
                    zones.Add(new Zone(x + 1, y + 1, bottom, right));
                }
                return zones;
            }
            else if (isWithin(x, top, bottom))
            {
                if (top < x)
                {
                    zones.Add(new Zone(top, left, x - 1, right));
                }
                if (bottom > x)
                {
                    zones.Add(new Zone(x + 1, left, bottom, right));
                }
                return zones;
            }
            else if (isWithin(y, left, right))
            {
                if (left < y)
                {
                    zones.Add(new Zone(top, left, bottom, y - 1));
                }
                if (right > y)
                {
                    zones.Add(new Zone(top, y + 1, bottom, right));
                }
                return zones;
            }

            zones.Add(this);
            return zones;
        }

        public static bool isWithin(int x, int low, int high)
        {
            return x >= low && x <= high;
        }
    }
}
