using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("H.txt"))
            {
                using (StreamWriter w = new StreamWriter("H-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {
                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');

                        int R = Convert.ToInt32(tokens[0]);
                        int C = Convert.ToInt32(tokens[1]);
                        int N = Convert.ToInt32(tokens[2]);

                        Console.WriteLine("Řeším úkol {0} z {1}, {2} x {3}; N = {4}", t + 1, T, R, C, N);

                        int zustatek = N;

                        int[,] myArr = new int[R, C];                //0-les;    1-poušť
                        //int[] remainingOpenCells = new int[R];
                        //for (int i = 0; i < remainingOpenCells.Length; i++)
                        //{
                        //    remainingOpenCells[i] = C;
                        //}

                        //for (int i = 0; i < N; i++)
                        //{
                        //    line = r.ReadLine();
                        //    tokens = line.Split(' ');
                        //    int xCord = Convert.ToInt32(tokens[0]) - 1;
                        //    int yCord = Convert.ToInt32(tokens[1]) - 1;

                        //    if (i > C && WeHaveToStopThis(xCord, yCord, remainingOpenCells, myArr))
                        //    {
                        //        zustatek--;
                        //        continue;
                        //    }
                        //    else
                        //    {
                        //        myArr[xCord, yCord] = 1;
                        //        remainingOpenCells[xCord]--;
                        //    }
                        //}

                        //for (int i = 0; i < N; i++)
                        //{
                        //    line = r.ReadLine();
                        //    tokens = line.Split(' ');
                        //    int xCord = Convert.ToInt32(tokens[0]) - 1;
                        //    int yCord = Convert.ToInt32(tokens[1]) - 1;

                        //    myArr[xCord, yCord]++;  
                        //}
                        //zustatek = N - NajdiNejlevnejsiCestu(myArr);

                        //for (int i = 0; i < N; i++)
                        //{
                        //    line = r.ReadLine();
                        //    tokens = line.Split(' ');
                        //    int xCord = Convert.ToInt32(tokens[0]) - 1;
                        //    int yCord = Convert.ToInt32(tokens[1]) - 1;

                        //    if (myArr[xCord, yCord] == 1)
                        //    {
                        //        continue;
                        //    }
                        //    else
                        //    {
                        //        myArr[xCord, yCord] = 1;
                        //        if (NajdiNejlevnejsiCestu(myArr) != 0)
                        //        {
                        //            zustatek--;
                        //            continue;
                        //        }
                        //        else
                        //        {
                        //            myArr[xCord, yCord] = 0;
                        //        }
                        //    }
                        //}

                        if (C == 1 || R == 1)
                        {
                            for (int i = 0; i < N; i++)
                            {
                                r.ReadLine();
                            }
                            zustatek = 0;
                            w.WriteLine(zustatek);
                            Console.WriteLine(zustatek);
                            continue;
                        }

                        List<int>[] cesta = new List<int>[2];
                        cesta[0] = new List<int>();
                        cesta[1] = new List<int>();
                        NajdiNovouCestu(myArr, cesta);
                        for (int i = 0; i < N; i++)
                        {
                            if (N > 100000 && i % 30000 == 0)
                            {
                                Console.WriteLine(i + "/" + N);
                            }
                            line = r.ReadLine();
                            tokens = line.Split(' ');
                            int xCord = Convert.ToInt32(tokens[0]) - 1;
                            int yCord = Convert.ToInt32(tokens[1]) - 1;

                            myArr[xCord, yCord] = 1;
                            int cestaIndex = ZasahujeDoCesty(xCord, yCord, cesta);
                            if (cestaIndex != -1)
                            {
                                if (NajdiQuickFix(cestaIndex, myArr, cesta))
                                {
                                    FixCesta(myArr, cesta);
                                }
                                else if (NajdiNovouCestu(myArr, cesta))
                                {

                                }
                                else
                                {
                                    zustatek--;
                                    myArr[xCord, yCord] = 0;
                                }
                            }
                            else
                            {

                            }
                        }

                        w.WriteLine(zustatek);
                        Console.WriteLine(zustatek);
                    }
                    Console.WriteLine("Hotovo");
                }
            }
        }

        static void FixCesta(int[,] myArr, List<int>[] cesta)
        {
            int index = cesta[0].IndexOf(myArr.GetLength(0) - 1, 0);
            if (index != -1 && index != cesta[0].Count - 1)
            {
                cesta[0].RemoveRange(index + 1, cesta[0].Count - index - 1);
                cesta[1].RemoveRange(index + 1, cesta[1].Count - index - 1);
            }

            index = cesta[0].LastIndexOf(0);
            if (index > 0)
            {
                cesta[0].RemoveRange(0, index);
                cesta[1].RemoveRange(0, index);
            }
        }

        static bool NajdiQuickFix(int index, int[,] myArr, List<int>[] cesta)
        {
            //první políčko. zkusit obejít z leva, nebo z prava.
            //doleva + nahoru, doprava + nahoru
            if (index == 0)
            {
                int xCord = cesta[0][1];
                int yCord = cesta[1][1];
                int leftCord = yCord - 1;
                int rightCord = yCord + 1;

                if (leftCord == -1)
                {
                    leftCord = myArr.GetLength(1) - 1;
                }
                if (rightCord == myArr.GetLength(1))
                {
                    rightCord = 0;
                }

                if (myArr[xCord, leftCord] != 1 && myArr[xCord - 1, leftCord] != 1)
                {
                    cesta[0].RemoveAt(index);
                    cesta[1].RemoveAt(index);
                    cesta[0].Insert(index, xCord);
                    cesta[1].Insert(index, leftCord);
                    cesta[0].Insert(index, xCord - 1);
                    cesta[1].Insert(index, leftCord);
                    return true;
                }

                if (myArr[xCord, rightCord] != 1 && myArr[xCord - 1, rightCord] != 1)
                {
                    cesta[0].RemoveAt(index);
                    cesta[1].RemoveAt(index);
                    cesta[0].Insert(index, xCord);
                    cesta[1].Insert(index, rightCord);
                    cesta[0].Insert(index, xCord - 1);
                    cesta[1].Insert(index, rightCord);
                    return true;
                }
            }
            //poslední políčko. zkusit obejít z leva, nebo z prava.
            //doleva + dolu, doprava + dolu
            else if (index == cesta[0].Count-1)
            {
                int xCord = cesta[0][index - 1];
                int yCord = cesta[1][index - 1];
                int leftCord = yCord - 1;
                int rightCord = yCord + 1;

                if (leftCord == -1)
                {
                    leftCord = myArr.GetLength(1) - 1;
                }
                if (rightCord == myArr.GetLength(1))
                {
                    rightCord = 0;
                }

                if (myArr[xCord, leftCord] != 1 && myArr[xCord + 1, leftCord] != 1)
                {
                    cesta[0].RemoveAt(index);
                    cesta[1].RemoveAt(index);
                    cesta[0].Add(xCord);
                    cesta[1].Add(leftCord);
                    cesta[0].Add(xCord + 1);
                    cesta[1].Add(leftCord);
                    return true;
                }

                if (myArr[xCord, rightCord] != 1 && myArr[xCord + 1, rightCord] != 1)
                {
                    cesta[0].RemoveAt(index);
                    cesta[1].RemoveAt(index);
                    cesta[0].Add(xCord);
                    cesta[1].Add(rightCord);
                    cesta[0].Add(xCord + 1);
                    cesta[1].Add(rightCord);
                    return true;
                }
            }
            else
            {
                int xCordNext = cesta[0][index + 1];
                int yCordNext = cesta[1][index + 1];
                int xCordPoust = cesta[0][index];
                int yCordPoust = cesta[1][index];
                int xCordPrev = cesta[0][index - 1];
                int yCordPrev = cesta[1][index - 1];
                //jestli můžeš utýct, tak uteč
                if (xCordNext == 1)
                {
                    if (myArr[xCordNext - 1, yCordNext] == 0)
                    {
                        cesta[0].RemoveRange(0, index);
                        cesta[1].RemoveRange(0, index);

                        cesta[0].Insert(0, xCordNext - 1);
                        cesta[1].Insert(0, yCordNext);
                    }
                }
                if (xCordNext == myArr.GetLength(0) - 2)
                {
                    if (myArr[xCordNext + 1, yCordNext] == 0)
                    {
                        cesta[0].RemoveRange(index + 2, cesta[0].Count - index - 2);
                        cesta[1].RemoveRange(index + 2, cesta[1].Count - index - 2);

                        cesta[0].Insert(index + 2, xCordNext + 1);
                        cesta[1].Insert(index + 2, yCordNext);
                    }
                }
                //kdykoli, pokud se poušť objeví v zatáčce, zkusit druhou možnost.
                //doleva + nahoru / nahoru + doleva ; doprava + nahoru / nahoru + doprava
                if (JeToZatacka(xCordPrev,yCordPrev,xCordNext,yCordNext, myArr.GetLength(1) - 1))
                {
                    if (xCordNext == xCordPoust)
                    {
                        if (myArr[xCordPrev,yCordNext] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            if (xCordPrev == 0)
                            {
                                cesta[0].RemoveAt(0);
                                cesta[1].RemoveAt(0);
                                index = 0;
                            }
                            cesta[0].Insert(index, xCordPrev);
                            cesta[1].Insert(index, yCordNext);
                            return true;
                        }
                    }
                    else
                    {
                        if (myArr[xCordNext, yCordPrev] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            cesta[0].Insert(index, xCordNext);
                            cesta[1].Insert(index, yCordPrev);
                            return true;
                        }
                    }
                }
                //kdykoli, pokud se poušť objeví mezi dvema body, zkusit poušť obejít.
                switch (JeToObkrok(xCordPrev, yCordPrev, xCordNext, yCordNext, myArr.GetLength(1)))
                {
                    case 1:
                        int leftCord = yCordPrev - 1;
                        int rightCord = yCordPrev + 1;
                        if (leftCord == -1)
                        {
                            leftCord += myArr.GetLength(1);
                        }
                        if (rightCord == myArr.GetLength(1))
                        {
                            rightCord = 0;
                        }

                        //doleva + nahoru + nahoru + doprava ; doprava + nahoru + nahoru + doleva
                        if (xCordPrev < xCordNext)
                        {
                            if (myArr[xCordPrev, leftCord] == 0 && myArr[xCordPrev + 1, leftCord] == 0 && myArr[xCordPrev + 2, leftCord] == 0)
                            {
                                cesta[0].RemoveAt(index);
                                cesta[1].RemoveAt(index);
                                if (xCordPrev == 0)
                                {
                                    cesta[0].RemoveAt(0);
                                    cesta[1].RemoveAt(0);
                                    index = 0;
                                }
                                else if (xCordNext == myArr.GetLength(0) - 1)
                                {
                                    cesta[0].RemoveAt(index);
                                    cesta[1].RemoveAt(index);
                                }
                                cesta[0].Insert(index, xCordPrev + 2);
                                cesta[1].Insert(index, leftCord);
                                cesta[0].Insert(index, xCordPrev + 1);
                                cesta[1].Insert(index, leftCord);
                                cesta[0].Insert(index, xCordPrev);
                                cesta[1].Insert(index, leftCord);
                                return true;
                            }
                            if (myArr[xCordPrev, rightCord] == 0 && myArr[xCordPrev + 1, rightCord] == 0 && myArr[xCordPrev + 2, rightCord] == 0)
                            {
                                cesta[0].RemoveAt(index);
                                cesta[1].RemoveAt(index);
                                if (xCordPrev == 0)
                                {
                                    cesta[0].RemoveAt(0);
                                    cesta[1].RemoveAt(0);
                                    index = 0;
                                }
                                else if (xCordNext == myArr.GetLength(0) - 1)
                                {
                                    cesta[0].RemoveAt(index);
                                    cesta[1].RemoveAt(index);
                                }
                                cesta[0].Insert(index, xCordPrev + 2);
                                cesta[1].Insert(index, rightCord);
                                cesta[0].Insert(index, xCordPrev + 1);
                                cesta[1].Insert(index, rightCord);
                                cesta[0].Insert(index, xCordPrev);
                                cesta[1].Insert(index, rightCord);
                                return true;
                            }
                        }
                        break;
                    case 2:
                        int leftCord1 = yCordPrev;
                        int leftCord2 = yCordPrev - 1;
                        int leftCord3 = yCordPrev - 2;
                        if (leftCord2 < 0)
                        {
                            leftCord2 += myArr.GetLength(1);
                        }
                        if (leftCord3 < 0)
                        {
                            leftCord3 += myArr.GetLength(1);
                        }

                        if (myArr[xCordPrev - 1, leftCord1] == 0 && myArr[xCordPrev - 1, leftCord2] == 0 && myArr[xCordPrev - 1, leftCord3] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, leftCord3);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, leftCord2);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, leftCord1);
                            return true;
                        }
                        if (myArr[xCordPrev + 1, leftCord1] == 0 && myArr[xCordPrev + 1, leftCord2] == 0 && myArr[xCordPrev + 1, leftCord3] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, leftCord3);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, leftCord2);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, leftCord1);
                            return true;
                        }
                        break;
                    case 3:
                        int rightCord1 = yCordPrev;
                        int rightCord2 = yCordPrev + 1;
                        int rightCord3 = yCordPrev + 2;
                        if (rightCord2 > myArr.GetLength(1) - 1)
                        {
                            rightCord2 -= myArr.GetLength(1);
                        }
                        if (rightCord3 > myArr.GetLength(1) - 1)
                        {
                            rightCord3 -= myArr.GetLength(1);
                        }

                        if (myArr[xCordPrev - 1, rightCord1] == 0 && myArr[xCordPrev - 1, rightCord2] == 0 && myArr[xCordPrev - 1, rightCord3] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, rightCord3);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, rightCord2);
                            cesta[0].Insert(index, xCordPrev - 1);
                            cesta[1].Insert(index, rightCord1);
                            return true;
                        }
                        if (myArr[xCordPrev + 1, rightCord1] == 0 && myArr[xCordPrev + 1, rightCord2] == 0 && myArr[xCordPrev + 1, rightCord3] == 0)
                        {
                            cesta[0].RemoveAt(index);
                            cesta[1].RemoveAt(index);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, rightCord3);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, rightCord2);
                            cesta[0].Insert(index, xCordPrev + 1);
                            cesta[1].Insert(index, rightCord1);
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        static int JeToObkrok(int xCordPrev, int yCordPrev, int xCordNext, int yCordNext, int maxLenght)
        {
            if (Math.Abs(xCordNext - xCordPrev) == 2 && Math.Abs(yCordNext - yCordPrev) == 0)
            {
                return 1;
            }
            int leftCord = yCordPrev - 2;
            int rightCord = yCordPrev + 2;
            if (leftCord < 0)
            {
                leftCord += maxLenght;
            }
            if (rightCord > maxLenght - 1)
            {
                rightCord -= maxLenght;
            }
            if (leftCord == yCordNext)
            {
                return 2;
            }
            if (rightCord == yCordNext)
            {
                return 3;
            }

            return -1;
        }

        static bool JeToZatacka(int xCordPrev, int yCordPrev, int xCordNext, int yCordNext, int maxLenght)
        {
            if (Math.Abs(xCordNext - xCordPrev) == 1)
            {
                if (Math.Abs(yCordNext-yCordPrev) == 1)
                {
                    return true;
                }
                else if ((yCordPrev == 0 && yCordNext == maxLenght) || (yCordPrev == maxLenght && yCordNext == 0))
                {
                    return true;
                }
            }
            return false;
        }

        static bool NajdiNovouCestu(int[,] myArr, List<int>[] cesta)
        {
            List<int> checkpoints = new List<int>();
            bool[,] connectionsArr = new bool[myArr.GetLength(0), myArr.GetLength(1)];
            FillConnections(myArr, connectionsArr);
            if (!ExistujeCesta(connectionsArr))
            {
                return false;
            }
            NaplnCheckpointy(myArr, connectionsArr, checkpoints);

            int[,] incomeX = new int[myArr.GetLength(0), myArr.GetLength(1)];
            int[,] incomeY = new int[myArr.GetLength(0), myArr.GetLength(1)];
            List<int>[] checkList = new List<int>[myArr.GetLength(0)];
            for (int i = 0; i < checkList.Length; i++)
            {
                checkList[i] = new List<int>();
            }

            bool[,] reachableArr = new bool[myArr.GetLength(0), myArr.GetLength(1)];

            for (int i = 0; i < myArr.GetLength(1); i++)
            {
                if (myArr[myArr.GetLength(0) - 1, i] == 0)
                {
                    reachableArr[myArr.GetLength(0) - 1, i] = true;
                    incomeX[myArr.GetLength(0) - 1, i] = myArr.GetLength(0);
                    incomeY[myArr.GetLength(0) - 1, i] = i;
                    if (myArr[myArr.GetLength(0) - 2, i] != 1)
                    {
                        checkList[myArr.GetLength(0) - 2].Add(i);
                    }
                }
            }

            bool finished = false;
            int topRow = myArr.GetLength(0) - 2;
            int limitRow = myArr.GetLength(0) - 2;
            int lastCheckpoint = myArr.GetLength(0) - 2;
            do
            {
                for (int i = limitRow; i < myArr.GetLength(0) - 1; i++)
                {
                    limitRow = -1;
                    bool changed = false;
                    if (ThisRowCheckpoint(i, connectionsArr, reachableArr))
                    {
                        lastCheckpoint = i;
                    }
                    for (int j = checkList[i].Count - 1; j >= 0; j--)
                    {
                        int column = checkList[i][j];
                        if (myArr[i, column] == 1 || reachableArr[i, column] == true)
                        {
                            checkList[i].RemoveAt(j);
                            continue;
                        }
                        if (TrySetInCome(i, column, reachableArr, incomeX, incomeY, myArr, checkList))
                        {
                            changed = true;
                            limitRow = i - 1;
                            if (checkpoints.Contains(i - 1))
                            {
                                lastCheckpoint = i - 1;
                            }
                            if (i == myArr.GetLength(0) - 2 || i == 0)
                            {
                                break;
                            }
                        }
                    }
                    if (changed)
                    {
                        if (i == topRow)
                        {
                            topRow--;
                        }
                        break;
                    }
                    //FAIL
                    if (i == lastCheckpoint)
                    {
                        finished = true;
                        break;
                    }
                }
                if (topRow == -1 || limitRow == -1)
                {
                    finished = true;
                }
            } while (!finished);

            int index = -1;
            for (int i = 0; i < myArr.GetLength(1); i++)
            {
                if (reachableArr[0, i])
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                return false;
            }

            cesta[0].Clear();
            cesta[1].Clear();
            int radek = 0;
            int sloupec = index;

            cesta[0].Add(radek);
            cesta[1].Add(sloupec);
            do
            {
                int meziRadek = radek;
                int meziSloupec = sloupec;

                radek = incomeX[meziRadek, meziSloupec];
                sloupec = incomeY[meziRadek, meziSloupec];
                
                cesta[0].Add(radek);
                cesta[1].Add(sloupec);
            } while (radek != myArr.GetLength(0) - 1);

            return true;
        }

        static bool ThisRowCheckpoint(int row, bool[,] connectionsArr, bool[,] reachableArr)
        {
            bool plati = true;
            int index = GetNextConnectionIndex(0, row, connectionsArr);
            while (index != -1)
            {
                if (reachableArr[row,index] == false)
                {
                    plati = false;
                    break;
                }
                index = GetNextConnectionIndex(index + 1, row, connectionsArr);
            }
            if (plati)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool ExistujeCesta(bool[,] connectionsArr)
        {
            for (int i = 1; i < connectionsArr.GetLength(0); i++)
            {
                bool projde = false;
                for (int j = 0; j < connectionsArr.GetLength(1); j++)
                {
                    if (connectionsArr[i,j])
                    {
                        projde = true;
                        break;
                    }
                }
                if (projde == false)
                {
                    return false;
                }
            }
            return true;
        }

        static void NaplnCheckpointy(int [,] myArr, bool[,] connectionsArr, List<int> checkpoints)
        {
            for (int i = 1; i < myArr.GetLength(0); i++)
            {
                if (TryCheckpoint(i,myArr,connectionsArr))
                {
                    checkpoints.Add(i);
                }
            }
        }

        static void FillConnections(int [,] myArr, bool[,] connectionsArr)
        {
            for (int i = 1; i < myArr.GetLength(0); i++)
            {
                for (int j = 0; j < myArr.GetLength(1); j++)
                {
                    if (myArr[i, j] != 1 && myArr[i - 1, j] != 1)
                    {
                        connectionsArr[i, j] = true;
                    }
                }
            }
        }

        static bool TrySetInCome(int xCord, int yCord, bool[,] reachableArr, int[,] incomeX, int[,] incomeY, int[,] myArr, List<int>[] checkList)
        {
            int leftCord = yCord - 1;
            int rightCord = yCord + 1;
            int upCord = xCord - 1;
            int downCord = xCord + 1;

            if (leftCord == -1)
            {
                leftCord += reachableArr.GetLength(1);
            }
            if (rightCord == reachableArr.GetLength(1))
            {
                rightCord -= reachableArr.GetLength(1);
            }

            //levo
            if (reachableArr[xCord, leftCord])
            {
                reachableArr[xCord, yCord] = true;
                incomeX[xCord, yCord] = xCord;
                incomeY[xCord, yCord] = leftCord;
                SetCheckList(xCord, yCord, leftCord, rightCord, upCord, downCord, myArr, reachableArr, checkList);
                return true;
            }

            //pravo
            if (reachableArr[xCord, rightCord])
            {
                reachableArr[xCord, yCord] = true;
                incomeX[xCord, yCord] = xCord;
                incomeY[xCord, yCord] = rightCord;
                SetCheckList(xCord, yCord, leftCord, rightCord, upCord, downCord, myArr, reachableArr, checkList);
                return true;
            }

            //nahoru
            if (upCord != -1)
            {
                if (reachableArr[upCord, yCord])
                {
                    reachableArr[xCord, yCord] = true;
                    incomeX[xCord, yCord] = upCord;
                    incomeY[xCord, yCord] = yCord;
                    SetCheckList(xCord, yCord, leftCord, rightCord, upCord, downCord, myArr, reachableArr, checkList);
                    return true;
                }
            }

            //dolu
            if (reachableArr[downCord, yCord])
            {
                reachableArr[xCord, yCord] = true;
                incomeX[xCord, yCord] = downCord;
                incomeY[xCord, yCord] = yCord;
                SetCheckList(xCord, yCord, leftCord, rightCord, upCord, downCord, myArr, reachableArr, checkList);
                return true;
            }

            return false;
        }

        static void SetCheckList(int xCord, int yCord, int leftCord, int rightCord, int upCord, int downCord, int[,] myArr, bool[,] reachableArr, List<int>[] checkList)
        {
            if (myArr[xCord,leftCord] == 0 && reachableArr[xCord, leftCord] == false)
            {
                if (checkList[xCord].Contains(leftCord) == false)
                {
                    checkList[xCord].Add(leftCord);
                    checkList[xCord].Sort();
                }
            }
            if (myArr[xCord, rightCord] == 0 && reachableArr[xCord, rightCord] == false)
            {
                if (checkList[xCord].Contains(rightCord) == false)
                {
                    checkList[xCord].Add(rightCord);
                    checkList[xCord].Sort();
                }
            }
            if (upCord != -1)
            {
                if (myArr[upCord, yCord] == 0 && reachableArr[upCord, yCord] == false)
                {
                    if (checkList[upCord].Contains(yCord) == false)
                    {
                        checkList[upCord].Add(yCord);
                        checkList[upCord].Sort();
                    }
                }
            }
            if (myArr[downCord, yCord] == 0 && reachableArr[downCord, yCord] == false)
            {
                if (checkList[downCord].Contains(yCord) == false)
                {
                    checkList[downCord].Add(yCord);
                    checkList[downCord].Sort();
                }
            }
        }

        static int ZasahujeDoCesty(int xCord, int yCord, List<int>[] cesta)
        {
            int index = cesta[0].IndexOf(xCord, 0);
            do
            {
                if (cesta[0][index] == xCord && cesta[1][index] == yCord)
                {
                    return index;
                }
                index = cesta[0].IndexOf(xCord, index+1);
            } while (index != -1);

            return -1;
        }

        static int NajdiNejlevnejsiCestu(int[,] myArr)
        {
            int nejlevnejsiCesta = int.MaxValue;
            int[,] nejlevnejsiCestaArr = new int[myArr.GetLength(0), myArr.GetLength(1)];

            int lowerRow = myArr.GetLength(0) - 2;

            for (int i = 0; i < myArr.GetLength(0); i++)
            {
                for (int j = 0; j < myArr.GetLength(1); j++)
                {
                    nejlevnejsiCestaArr[i, j] = -1;
                }
            }
            for (int sloupec = 0; sloupec < myArr.GetLength(1); sloupec++)
            {
                for (int i = myArr.GetLength(0)-1; i >= 0; i--)
                {
                    if (i == myArr.GetLength(0) - 1)
                    {
                        nejlevnejsiCestaArr[i, sloupec] = myArr[i, sloupec];
                    }
                    else
                    {
                        nejlevnejsiCestaArr[i, sloupec] = nejlevnejsiCestaArr[i + 1, sloupec] + myArr[i, sloupec];
                    }
                    if (i == 0)
                    {
                        if (nejlevnejsiCestaArr[i, sloupec] == 0)
                        {
                            return 0;
                        }
                    }
                    if (myArr[i,sloupec] == 1)
                    {
                        break;
                    }
                }
            }

            do
            {
                
            } while (lowerRow != 0);

            for (int i = 0; i < myArr.GetLength(1); i++)
            {
                if (nejlevnejsiCestaArr[0, i] < nejlevnejsiCesta)
                {
                    nejlevnejsiCesta = nejlevnejsiCestaArr[0, i];
                }
            }

            return nejlevnejsiCesta;
        }

        static bool TrySetCheapestWay(int xCord, int yCord, int[,] myArr, int[,] nejlevnejsiCestaArr)
        {
            int nejlevnejsiCesta = int.MaxValue;
            int leftCord = yCord - 1;
            int rightCord = yCord + 1;
            int upCord = xCord - 1;
            int downCord = xCord + 1;

            if (leftCord == -1)
            {
                leftCord += nejlevnejsiCestaArr.GetLength(1);
            }
            if (rightCord == nejlevnejsiCestaArr.GetLength(1))
            {
                rightCord -= nejlevnejsiCestaArr.GetLength(1);
            }

            //levo
            if (nejlevnejsiCestaArr[xCord,leftCord] != -1)
            {
                if (nejlevnejsiCestaArr[xCord, leftCord] + myArr[xCord, leftCord] < nejlevnejsiCesta)
                {
                    nejlevnejsiCesta = nejlevnejsiCestaArr[xCord, leftCord] + myArr[xCord, leftCord];
                }
            }

            //pravo
            if (nejlevnejsiCestaArr[xCord, rightCord] != -1)
            {
                if (nejlevnejsiCestaArr[xCord, rightCord] + myArr[xCord, rightCord] < nejlevnejsiCesta)
                {
                    nejlevnejsiCesta = nejlevnejsiCestaArr[xCord, rightCord] + myArr[xCord, rightCord];
                }
            }

            //nahoru
            if (upCord != -1)
            {
                if (nejlevnejsiCestaArr[upCord, yCord] != -1)
                {
                    if (nejlevnejsiCestaArr[upCord, yCord] + myArr[upCord, yCord] < nejlevnejsiCesta)
                    {
                        nejlevnejsiCesta = nejlevnejsiCestaArr[upCord, yCord] + myArr[upCord, yCord];
                    }
                }
            }

            //dolu
            if (nejlevnejsiCestaArr[downCord, yCord] != -1)
            {
                if (nejlevnejsiCestaArr[downCord, yCord] + myArr[downCord, yCord] < nejlevnejsiCesta)
                {
                    nejlevnejsiCesta = nejlevnejsiCestaArr[downCord, yCord] + myArr[downCord, yCord];
                }
            }

            if (nejlevnejsiCesta != int.MaxValue)
            {
                if (nejlevnejsiCestaArr[xCord, yCord] == -1 || nejlevnejsiCesta < nejlevnejsiCestaArr[xCord, yCord])
                {
                    nejlevnejsiCestaArr[xCord, yCord] = nejlevnejsiCesta;
                    return true;
                }
            }
            return false;
        }

        static bool WeHaveToStopThis(int xCord, int yCord, int[] remainingOpenCells, int[,] myArr)
        {
            if (myArr[xCord,yCord] == 1)
            {
                Console.WriteLine("What?");
                return false;
            }
            if (remainingOpenCells[xCord] == 1)
            {
                return true;
            }
            bool[,] connections = new bool[myArr.GetLength(0),myArr.GetLength(1)];
            if (!EveryRowConnectsLower(xCord, yCord, myArr, connections))
            {
                return true;
            }
            if (!DoesAWayExist(xCord, yCord, myArr, connections))
            {
                return true;
            }

            return false;
        }

        static bool DoesAWayExist(int xCord, int yCord, int[,] myArr, bool[,] connectionsArr)
        {
            myArr[xCord, yCord] = 1;
            bool wayExists = false;
            List<int> rozcestiX = new List<int>();
            List<int> rozcestiY = new List<int>();
            List<int> rozcestiSmer = new List<int>();
            int bestRow = 0;
            int row = 0;
            int column = -1;
            int smer = 0;
            for (int i = 0; i < GetNumberOfConnections(row, connectionsArr); i++)
            {
                column = GetNextConnectionIndex(column + 1, row, connectionsArr);
                rozcestiX.Add(row);
                rozcestiY.Add(column);
                rozcestiSmer.Add(2);
            }
            do
            {
                if (rozcestiX.Count == 0)
                {
                    break;
                }

                row = rozcestiX[rozcestiX.Count - 1];
                column = rozcestiY[rozcestiY.Count - 1];
                smer = rozcestiSmer[rozcestiSmer.Count - 1];

                if (row > bestRow)
                {
                    bestRow = row;
                    if (TryCheckpoint(row, myArr, connectionsArr))
                    {
                        rozcestiX = new List<int>();
                        rozcestiY = new List<int>();
                        rozcestiSmer = new List<int>();
                        column = -1;
                        for (int i = 0; i < GetNumberOfConnections(row, connectionsArr); i++)
                        {
                            column = GetNextConnectionIndex(column + 1, row, connectionsArr);
                            rozcestiX.Add(row);
                            rozcestiY.Add(column);
                            rozcestiSmer.Add(2);
                        }

                        row = rozcestiX[rozcestiX.Count - 1];
                        column = rozcestiY[rozcestiY.Count - 1];
                        smer = rozcestiSmer[rozcestiSmer.Count - 1];
                    }
                }

                rozcestiX.RemoveAt(rozcestiX.Count - 1);
                rozcestiY.RemoveAt(rozcestiY.Count - 1);
                rozcestiSmer.RemoveAt(rozcestiSmer.Count - 1);

                if (row == myArr.GetLength(0) - 2 && smer == 2)
                {
                    wayExists = true;
                    break;
                }


                switch (smer)
                {
                    case 1:
                        FindConnectionToLeft(row, column, myArr, connectionsArr, rozcestiX, rozcestiY, rozcestiSmer);
                        break;
                    case 2:
                        FindAWay(row + 1, column, myArr, 4, rozcestiX, rozcestiY, rozcestiSmer);
                        break;
                    case 3:
                        FindConnectionToRight(row, column, myArr, connectionsArr, rozcestiX, rozcestiY, rozcestiSmer);
                        break;
                }
            } while (true);

            //bool[,] reachable = new bool[myArr.GetLength(0), myArr.GetLength(1)];
            //bool[,] reachFin = new bool[myArr.GetLength(0), myArr.GetLength(1)];
            //int lastRow = 0;
            //int countOfUnFin = 0;
            //for (int i = 0; i < reachable.GetLength(1); i++)
            //{
            //    if (myArr[0,i] == 0)
            //    {
            //        reachable[0, i] = true;
            //        countOfUnFin++;
            //    }
            //}
            //do
            //{
            //    for (int i = 0; i <= lastRow; i++)
            //    {
            //        for (int j = 0; j < reachable.GetLength(1); j++)
            //        {
            //            if (reachable[i, j] && reachFin[i, j] == false)
            //            {
            //                int row;
            //                int column;

            //                column = j - 1;
            //                if (column == -1)
            //                {
            //                    column += reachable.GetLength(1);
            //                }
            //                if (myArr[i, column] == 0)
            //                {
            //                    reachable[i, column] = true;
            //                }

            //                row = i - 1;
            //                if (row != -1)
            //                {
            //                    if (myArr[row, j] == 0)
            //                    {
            //                        reachable[row, j] = true;
            //                    }
            //                }

            //                column = j + 1;
            //                if (column == reachable.GetLength(1))
            //                {
            //                    column -= reachable.GetLength(1);
            //                }
            //                if (myArr[i, column] == 0)
            //                {
            //                    reachable[i, column] = true;
            //                }

            //                row = i + 1;
            //                if (row != myArr.GetLength(0) - 1)
            //                {
            //                    if (myArr[row, j] == 0)
            //                    {
            //                        reachable[row, j] = true;
            //                        if (row > lastRow)
            //                        {
            //                            lastRow = row;
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    break;
            //                }

            //                reachFin[i, j] = true;
            //            }
            //        }
            //    }

            //    if (lastRow == myArr.GetLength(0)-1)
            //    {
            //        wayExists = true;
            //        break;
            //    }
            //    if (countOfUnFin == 0)
            //    {
            //        break;
            //    }
            //} while (true);


            myArr[xCord, yCord] = 0;
            if (wayExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool TryCheckpoint(int row, int[,] myArr, bool[,] connectionsArr)
        {
            int totalConnections = GetNumberOfConnections(row, connectionsArr);
            int foundConnections = 1;
            int startIndex = GetNextConnectionIndex(0, row, connectionsArr);

            int index = startIndex;
            while (myArr[row, index] == 0)
            {
                index--;
                if (index == -1)
                {
                    index += myArr.GetLength(1);
                }
                if (index == startIndex)
                {
                    return true;
                }
                if (connectionsArr[row, index])
                {
                    foundConnections++;
                }
            }
            index = startIndex;
            while (myArr[row, index] == 0)
            {
                index++;
                if (index == myArr.GetLength(1))
                {
                    index -= myArr.GetLength(1);
                }
                if (index == startIndex)
                {
                    return true;
                }
                if (connectionsArr[row, index])
                {
                    foundConnections++;
                }
            }

            if (foundConnections == totalConnections)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void FindConnectionToLeft(int xCord, int yCord, int[,] myArr, bool[,] connectionsArr, List<int> rozcestiX, List<int> rozcestiY, List<int> rozcestiSmer)
        {
            while (myArr[xCord,yCord] == 1)
            {
                yCord--;
                if (yCord == -1)
                {
                    yCord += myArr.GetLength(1);
                }
                if (connectionsArr[xCord, yCord])
                {
                    FindAWay(xCord,yCord,myArr,3,rozcestiX,rozcestiY,rozcestiSmer);
                    break;
                }
            }
        }

        static void FindConnectionToRight(int xCord, int yCord, int[,] myArr, bool[,] connectionsArr, List<int> rozcestiX, List<int> rozcestiY, List<int> rozcestiSmer)
        {
            while (myArr[xCord, yCord] == 1)
            {
                yCord++;
                if (yCord == myArr.GetLength(1))
                {
                    yCord -= myArr.GetLength(1);
                }
                if (connectionsArr[xCord, yCord])
                {
                    FindAWay(xCord, yCord, myArr, 1, rozcestiX, rozcestiY, rozcestiSmer);
                    break;
                }
            }
        }

        static void FindAWay(int xCord, int yCord, int[,] myArr, int direction, List<int> rozcestiX, List<int> rozcestiY, List<int> rozcestiSmer)
        {
            int leftCord = yCord - 1;
            int rightCord = yCord + 1;

            if (leftCord == -1)
            {
                leftCord += myArr.GetLength(1);
            }
            if (rightCord == myArr.GetLength(1))
            {
                rightCord -= myArr.GetLength(1);
            }

            if (myArr[xCord, leftCord] == 0 && direction != 1)
            {
                rozcestiX.Add(xCord);
                rozcestiY.Add(yCord);
                rozcestiSmer.Add(1);
            }
            if (myArr[xCord, rightCord] == 0 && direction != 3)
            {
                rozcestiX.Add(xCord);
                rozcestiY.Add(yCord);
                rozcestiSmer.Add(3);
            }
            if (myArr[xCord + 1, yCord] == 0 && direction != 2)
            {
                rozcestiX.Add(xCord);
                rozcestiY.Add(yCord);
                rozcestiSmer.Add(2);
            }
        }

        static int GetNumberOfConnections(int row, bool[,] connectionsArr)
        {
            int numberOfConnections = 0;
            for (int i = 0; i < connectionsArr.GetLength(1); i++)
            {
                if (connectionsArr[row, i] == true)
                {
                    numberOfConnections++;
                }
            }
            return numberOfConnections;
        }

        static int GetNextConnectionIndex(int startIndex, int row, bool[,] connectionsArr)
        {
            for (int i = startIndex; i < connectionsArr.GetLength(1); i++)
            {
                if (connectionsArr[row, i])
                {
                    return i;
                }
            }
            return -1;
        }

        static bool EveryRowConnectsLower(int xCord, int yCord, int[,] myArr, bool[,] connectionsArr)
        {
            bool connectionExist = true;
            myArr[xCord, yCord] = 1;

            for (int i = 0; i < myArr.GetLength(0) - 1; i++)
            {
                bool rowConnects = false;
                for (int j = 0; j < myArr.GetLength(1); j++)
                {
                    if (myArr[i, j] == 0 && myArr[i + 1, j] == 0)
                    {
                        rowConnects = true;
                        connectionsArr[i, j] = true;
                    }
                    else
                    {
                        connectionsArr[i, j] = false;
                    }
                }
                if (!rowConnects)
                {
                    connectionExist = false;
                    break;
                }
            }
            myArr[xCord, yCord] = 0;
            return connectionExist;
        }
    }
}