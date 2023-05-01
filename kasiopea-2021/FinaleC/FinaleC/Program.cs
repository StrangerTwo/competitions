using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleC
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("C.txt"))
            {
                using (StreamWriter w = new StreamWriter("C-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());

                        List<Station> stations = new List<Station>();

                        for (int i = 0; i < N; i++)
                        {
                            var coords = r.ReadLine().Split(' ').Select(x => Convert.ToInt64(x)).ToArray();
                            stations.Add(new Station(coords[0], coords[1]));
                        }

                        w.WriteLine(GetResult(N, stations));
                    }
                }
            }
        }

        private static string GetResult(int n, List<Station> stations)
        {
            var orderedStations = stations.OrderBy(x => x.y).ThenBy(x => x.x).ToArray();

            Dictionary<long, byte> jammedRows = new Dictionary<long, byte>();

            long currentColumn = 1;
            byte currentJamming = 0;
            foreach (var station in orderedStations)
            {
                if (station.y != currentColumn)
                {
                    currentColumn = station.y;
                    currentJamming = 0;
                }

                List<byte> availableFrequencies = new List<byte>(new byte[] { 1, 2, 3 });
                if (currentJamming != 0)
                {
                    availableFrequencies.Remove(currentJamming);
                }
                if (jammedRows.ContainsKey(station.x))
                {
                    availableFrequencies.Remove(jammedRows[station.x]);
                }

                station.frequency = availableFrequencies[0];
                jammedRows[station.x] = availableFrequencies[0];
                currentJamming = availableFrequencies[0];
            }

            return string.Join(" ", stations.Select(x => x.frequency));
        }
    }

    public class Station
    {
        public long x;
        public long y;
        public byte frequency;

        public Station(long x, long y)
        {
            this.x = x;
            this.y = y;
            frequency = 0;
        }
    }
}
