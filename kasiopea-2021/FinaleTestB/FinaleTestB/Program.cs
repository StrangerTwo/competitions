using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaleTestB
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(Directory.GetCurrentDirectory());
            using (StreamReader r = new StreamReader("B.txt"))
            {
                using (StreamWriter w = new StreamWriter("B-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);

                        int N = Convert.ToInt32(r.ReadLine());

                        var heights = r.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();


                        w.WriteLine(GetResult(heights));
                    }
                }
            }
        }

        private static int GetResult(int[] heights)
        {
            var sections = getSections(heights);
            for (int i = 0; i < 10; i++)
            {
                sections = getSections(sections);
            }
            
            return sections.Select(x => getMinOfColumns(x) * x.Count()).Max();
        }

        private static IEnumerable<IEnumerable<int>> getSections(IEnumerable<IEnumerable<int>> sections)
        {
            foreach (var section in sections)
            {
                foreach (var item in getSections(section))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<IEnumerable<int>> getSections(IEnumerable<int> heights)
        {
            int average = (int)heights.Average();

            List<int> section = new List<int>();
            foreach (var item in heights)
            {
                if (item < 0.3 * average)
                {
                    yield return section;
                }
                section.Add(item);
            }
            yield return section;
        }

        private static int getMinOfColumns(IEnumerable<int> heights)
        {
            int min = heights.First();
            foreach (var item in heights)
            {
                min = Math.Min(min, item);
            }
            return min;
        }
    }
}
