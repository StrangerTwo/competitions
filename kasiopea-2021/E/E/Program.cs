using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("E.txt"))
            {
                using (StreamWriter w = new StreamWriter("E-vysledek.txt"))
                {
                    int T = Convert.ToInt32(r.ReadLine());

                    for (int xNo = 1; xNo <= T; xNo++)
                    {
                        int N = Convert.ToInt32(r.ReadLine());
                        string a = r.ReadLine();
                        string b = r.ReadLine();
                        string c = r.ReadLine();

                        Console.WriteLine("Pracuji na příkladu {0} ze {1}", xNo, T);
                        checked
                        {
                            w.WriteLine(GetResult(a, b, c));
                        }
                    }

                    Process.Start(Directory.GetCurrentDirectory());
                }
            }
        }

        private static long GetResult(string a, string b, string c)
        {
            long changes = 0;

            int tenOver = 0;
            for (int i = a.Length - 1; i >= 0; i--)
            {
                int aN = (int)Char.GetNumericValue(a[i]);
                int bN = (int)Char.GetNumericValue(b[i]);
                int cN = (int)Char.GetNumericValue(c[i]);

                if (i == 0)
                {
                    changes += getChanges(aN, bN, cN, ref tenOver);
                }
                else
                {
                    int aNext = (int)Char.GetNumericValue(a[i - 1]);
                    int bNext = (int)Char.GetNumericValue(b[i - 1]);
                    int cNext = (int)Char.GetNumericValue(c[i - 1]);

                    if (i == 1)
                    {
                        changes += getChanges(aN, bN, cN, aNext, bNext, cNext, ref tenOver, true);
                    }
                    else
                    {
                        changes += getChanges(aN, bN, cN, aNext, bNext, cNext, ref tenOver, false);
                    }
                }
            }

            //changes = ChangesGet(a, b, c, a.Length);

            return changes;
        }

        private static int ChangesGet(string a, string b, string c, int length)
        {
            int substract;
            int changes = 0;
            while (true)
            {
                substract = Math.Abs(Convert.ToInt32(c) - Convert.ToInt32(a) - Convert.ToInt32(b));
                if (CheckForHelp(substract.ToString(), ref a, ref b, ref c, ref changes))
                {
                    continue;
                }

                break;
            }
            changes += getNumberOfNotZeros(substract.ToString());

            if (changes.ToString().Length > length)
            {
                changes++;
            }

            return changes;
        }

        private static bool CheckForHelp(string substract, ref string a, ref string b, ref string c, ref int changes)
        {
            for (int i = 1; i < substract.Length; i++)
            {
                if (substract[substract.Length - 1 - i] == '9')
                {
                    // Pokusíme se pomoct
                    int nA = Convert.ToInt32(a[a.Length - i]);
                    int nB = Convert.ToInt32(b[b.Length - i]);
                    int nC = Convert.ToInt32(c[c.Length - i]);

                    // V dalším čísle chybí desítka - zkusíme u sebe přidat
                    if (nA + nB >= 10)
                    {
                    }
                    else
                    {
                        // nešlo nic
                        continue;
                    }
                }
            }
            return false;
        }

        private static int getNumberOfNotZeros(string v)
        {
            int result = 0;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] != '0')
                {
                    result++;
                }
            }
            return result;
        }

        private static int getChanges(int a, int b, int c, ref int tenOver)
        {
            int sum = a + b + tenOver;
            int num = sum % 10;

            if (sum == c)
            {
                // OK, pohoda
                return 0;
            }
            else
            {
                if (sum < 10)
                {
                    // sum <= 9, takže stačí změnit výsledek
                    return 1;
                }
                else
                {
                    // sum >= 10
                    if (sum - a <= c || sum - b <= c)
                    {
                        // lze vyměnit číslo a,b za 0, abychom nešli přes 10
                        return 1;
                    }
                    else
                    {
                        // nejde změnit pouze jedno číslo
                        if (tenOver > 0)
                        {
                            // pokud máme 10 z minule, tak si musíme dát pozor na 0 ve výsledku -> musíme dát všude a,b 0 a c 1
                            if (c == 0)
                            {
                                return 3;
                            }
                            else
                            {
                                return 2;
                            }
                        }
                        else
                        {
                            // bude nám stačit změnit jen 2 čísla a,b
                            return 2;
                        }
                    }
                }
            }
        }

        private static int getChanges(int a, int b, int c, int aNext, int bNext, int cNext, ref int tenOver, bool nextIsLast)
        {
            int sum = a + b + tenOver;
            tenOver = (sum / 10);
            int num = sum % 10;

            if (num == c)
            {
                return 0;
            }
            else
            {
                // Pokud nám v dalším čísle chybí 1, tak zvedneme u nás
                int sumNext = aNext + bNext + tenOver;
                int numNext = sumNext % 10;

                if (numNext == cNext)
                {
                    // vyřešit pouze změnou u nás -> výsledek
                    return 1;
                }
                else
                {
                    if (tenOver == 0)
                    {
                        if (nextIsLast)
                        {
                            if (cNext == 0)
                            {
                                // nepřidávat 10
                                return 1;
                            }
                        }
                        // podívat se, jestli pomůžeme, když půjdeme přes 10
                        if ((sumNext + 1) % 10 == cNext)
                        {
                            // můžeme pomoct, podíváme se jestli můžeme jednoduše přejít 10
                            if (sum + (9 - a) >= c + 10 || sum + (9 - b) >= c + 10)
                            {
                                // můžeme pomoct jednoduše, změníme 1 číslo a půjdeme přes 10
                                tenOver = 1;
                                return 1;
                            }
                            else
                            {
                                //nemůžeme pomoct jednoduše, kašleme na něj -> výsledek
                                //return 1;
                                // EDIT: POMUZEME
                                if (c == 9 && sum != a + b)
                                {
                                    return 1;
                                }
                                tenOver = 1;
                                return 2;
                            }
                        }
                        else
                        {
                            // nepomohli, vyřešíme si to u nás -> výsledek
                            return 1;
                        }
                    }
                    else
                    {
                        // podívat se, jestli pomůžeme, když nepůjdeme přes 10
                        if ((sumNext - 1) % 10 == cNext)
                        {
                            // můžeme pomoct, podíváme se jestli můžeme jednoduše nepřejít 10
                            if (sum - a <= c || sum - b <= c)
                            {
                                // můžeme pomoct jednoduše, změníme 1 číslo a nepůjdeme přes 10
                                tenOver = 0;
                                return 1;
                            }
                            else
                            {
                                //nemůžeme pomoct jednoduše, kašleme na něj -> výsledek
                                //return 1;
                                // EDIT: POMUZEME
                                if (c == 0 && sum != a + b)
                                {
                                    return 1;
                                }
                                tenOver = 0;
                                return 2;
                            }
                        }
                        else
                        {
                            // nepomohli, vyřešíme si to u nás -> výsledek
                            return 1;
                        }
                    }
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
    }
}
