using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FinaleD
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("D.txt"))
            {
                using (StreamWriter w = new StreamWriter("D-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());

                        List<coord> coords = new List<coord>(N);
                        for (int i = 1; i <= N; i++)
                        {
                            var data = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
                            coords.Add(new coord(i, data[0], data[1]));
                        }

                        w.WriteLine(GetResult(N, coords));
                    }
                }
            }
        }

        private static string GetResult(int n, List<coord> coords)
        {
            coord current = new coord(-1, 0, 0);    // Pošta

            //direction je úhel
            double direction = 0; // začneme směrem nahoru, pomalu otáčíme doprava

            coord[] order = new coord[n];

            for (int i = 0; i < n; i++)
            {
                var next = current.getNext(coords, direction);
                order[i] = next;
                direction = coord.fixAngle(next.getAngle(current));
                coords.Remove(next);
                current = next;
            }

            return string.Join(" ", order.Select(x => x.index));
        }
    }

    public class coord
    {
        public int index;
        public int x;
        public int y;

        public coord(int i, int x, int y)
        {
            this.index = i;
            this.x = x;
            this.y = y;
        }

        public coord getNext(IEnumerable<coord> coords, double direction)
        {
            // Potřebujeme najít první souřadnici, která je napravo od našeho směru.
            var rotationOrder = coords.OrderBy(x => fixAngle(x.getAngle(this) - direction));

            return rotationOrder.First();
        }

        public static double fixAngle(double angle)
        {
            while (angle < 0)
            {
                angle += 360;
            }
            while (angle > 360)
            {
                angle -= 360;
            }
            return angle;
        }

        public double getAngle(coord from)
        {
            // 4 možnosti
            double Dx;
            double Dy;

            if (from.y < this.y && from.x <= this.x)
            {
                Dx = this.x - from.x;
                Dy = this.y - from.y;
                double angle = Math.Atan(Dx / Dy) * (180 / Math.PI);

                return angle;
            }
            if (from.y >= this.y && from.x < this.x)
            {
                Dx = this.x - from.x;
                Dy = from.y - this.y;
                double angle = Math.Atan(Dy / Dx) * (180 / Math.PI) + 90;

                return angle;
            }
            if (from.y > this.y && from.x >= this.x)
            {
                Dx = from.x - this.x;
                Dy = from.y - this.y;
                double angle = Math.Atan(Dx / Dy) * (180 / Math.PI) + 180;

                return angle;
            }
            else
            {
                Dx = from.x - this.x;
                Dy = this.y - from.y;
                double angle = Math.Atan(Dy / Dx) * (180 / Math.PI) + 270;

                return angle;
            }
        }
    }
}
