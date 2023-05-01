using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("G.txt"))
            {
                using (StreamWriter w = new StreamWriter("G-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {

                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');

                        int N = Convert.ToInt32(tokens[0]);
                        int M = Convert.ToInt32(tokens[1]);

                        char[,] myArr = new char[N, M];
                        char[,] finishArr = new char[N, M];
                        List<int>[] theLog = new List<int>[2];
                        theLog[0] = new List<int>();
                        theLog[1] = new List<int>();
                        List<int>[] theRevLog = new List<int>[2];
                        theRevLog[0] = new List<int>();
                        theRevLog[1] = new List<int>();

                        Console.WriteLine("Řeším úkol {0} z {1}, {2} x {3}", t + 1, T, N, M);

                        //TAKING THE INFO IN
                        for (int i = 0; i < N; i++)
                        {
                            line = r.ReadLine();
                            char[] tempArr = line.ToCharArray();

                            for (int j = 0; j < M; j++)
                            {
                                myArr[i, j] = tempArr[j];
                            }
                        }
                        r.ReadLine();
                        for (int i = 0; i < N; i++)
                        {
                            line = r.ReadLine();
                            char[] tempArr = line.ToCharArray();

                            for (int j = 0; j < M; j++)
                            {
                                finishArr[i, j] = tempArr[j];
                            }
                        }
                        r.ReadLine();

                        do
                        {
                            //START THE OPERATION
                            if (M % 2 == 0)
                            {
                                CalculateEverythingEven(myArr, finishArr, N, M, theLog, theRevLog);
                            }
                            else
                            {
                                CalculateEverythingOdd(myArr, finishArr, N, M, theLog, theRevLog);
                            }
                        } while (!CheckArrays(myArr,finishArr));

                        theRevLog[0].Reverse();
                        theRevLog[1].Reverse();


                        w.WriteLine(theLog[0].Count + theRevLog[0].Count);
                        for (int i = 0; i < theLog[0].Count; i++)
                        {
                            w.WriteLine(theLog[0][i] + " " + theLog[1][i]);
                        }
                        for (int i = 0; i < theRevLog[0].Count; i++)
                        {
                            w.WriteLine(theRevLog[0][i] + " " + theRevLog[1][i]);
                        }

                    }
                }
            }
        }

        static bool CheckArrays(char[,] myArr, char[,] finishArr)
        {
            for (int i = 0; i < myArr.GetLength(0); i++)
            {
                for (int j = 0; j < myArr.GetLength(1); j++)
                {
                    if (myArr[i,j] != finishArr[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void CalculateEverythingEven(char[,] myArr, char[,] finishArr, int N, int M, List<int>[] theLog, List<int>[] theRevLog)
        {
            int finishedColumn = -1;
            int finishedRow = -1;
            List<int>[] changed = new List<int>[2];
            changed[0] = new List<int>();
            changed[1] = new List<int>();
            bool changeAnything = false;
            for (int column = 0; column < M; column++)
            {
                for (int row = 0; row < N; row++)
                {
                    if (column == -1)
                    {
                        break;
                    }
                    if (changeAnything)
                    {
                        if (CanChangeHere(ref myArr, row, column))
                        {
                            if (!ThisAlready(row, column, changed))
                            {
                                TurnPieces(ref myArr, row, column, theLog, changed);
                                column = finishedColumn;
                                row = finishedRow;
                                changeAnything = false;
                            }
                        }
                    }
                    else
                    {
                        if (row != N - 1)
                        {
                            if (myArr[row, column] == 'U' || myArr[row, column] == 'D')
                            {
                                finishedColumn = column;
                                finishedRow = row;
                                changed[0] = new List<int>();
                                changed[1] = new List<int>();
                            }
                            else
                            {
                                changeAnything = true;
                                row--;
                            }
                        }
                    }
                }
            }

            finishedColumn = -1;
            finishedRow = -1;
            changeAnything = false;
            for (int column = 0; column < M; column++)
            {
                for (int row = 0; row < N; row++)
                {
                    if (column == -1)
                    {
                        break;
                    }
                    if (changeAnything)
                    {
                        if (CanChangeHere(ref finishArr, row, column))
                        {
                            if (!ThisAlready(row, column, changed))
                            {
                                TurnPieces(ref finishArr, row, column, theRevLog, changed);
                                column = finishedColumn;
                                row = finishedRow;
                                changeAnything = false;
                            }
                        }
                    }
                    else
                    {
                        if (row != N - 1)
                        {
                            if (finishArr[row, column] == 'U' || finishArr[row, column] == 'D')
                            {
                                finishedColumn = column;
                                finishedRow = row;
                                changed[0] = new List<int>();
                                changed[1] = new List<int>();
                            }
                            else
                            {
                                changeAnything = true;
                                row--;
                            }
                        }
                    }
                }
            }
        }

        static void CalculateEverythingOdd(char[,] myArr, char[,] finishArr, int N, int M, List<int>[] theLog, List<int>[] theRevLog)
        {
            int finishedColumn = -1;
            int finishedRow = -1;
            List<int>[] changed = new List<int>[2];
            changed[0] = new List<int>();
            changed[1] = new List<int>();
            bool changeAnything = false;
            for (int row = 0; row < N; row++)
            {
                for (int column = 0; column < M; column++)
                {
                    if (row == -1)
                    {
                        break;
                    }
                    if (changeAnything)
                    {
                        if (CanChangeHere(ref myArr, row, column))
                        {
                            if (!ThisAlready(row, column, changed))
                            {
                                TurnPieces(ref myArr, row, column, theLog, changed);
                                column = finishedColumn;
                                row = finishedRow;
                                changeAnything = false;
                            }
                        }
                    }
                    else
                    {
                        if (column != M - 1)
                        {
                            if (myArr[row, column] == 'L' || myArr[row, column] == 'R')
                            {
                                finishedColumn = column;
                                finishedRow = row;
                                changed[0] = new List<int>();
                                changed[1] = new List<int>();
                            }
                            else
                            {
                                changeAnything = true;
                                column--;
                            }
                        }
                    }
                }
            }

            finishedColumn = -1;
            finishedRow = -1;
            changeAnything = false;
            for (int row = 0; row < N; row++)
            {
                for (int column = 0; column < M; column++)
                {
                    if (row == -1)
                    {
                        break;
                    }
                    if (changeAnything)
                    {
                        if (CanChangeHere(ref finishArr, row, column))
                        {
                            if (!ThisAlready(row, column, changed))
                            {
                                TurnPieces(ref finishArr, row, column, theRevLog, changed);
                                column = finishedColumn;
                                row = finishedRow;
                                changeAnything = false;
                            }
                        }
                    }
                    else
                    {
                        if (column != M - 1)
                        {
                            if (finishArr[row, column] == 'L' || finishArr[row, column] == 'R')
                            {
                                finishedColumn = column;
                                finishedRow = row;
                                changed[0] = new List<int>();
                                changed[1] = new List<int>();
                            }
                            else
                            {
                                changeAnything = true;
                                column--;
                            }
                        }
                    }
                }
            }
        }

        static bool ThisAlready(int xCord, int yCord, List<int>[] changed)
        {
            for (int i = 0; i < changed[0].Count; i++)
            {
                if (changed[0][i] == xCord)
                {
                    if (changed[1][i] == yCord)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool CanChangeHere(ref char[,] testArr, int xCord, int yCord)
        {
            if (xCord + 1 >= testArr.GetLength(0) || yCord + 1 >= testArr.GetLength(1))
            {
                return false;
            }
            switch (testArr[xCord, yCord])
            {
                case 'L':
                    if (testArr[xCord + 1, yCord] == 'L')
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 'U':
                    if (testArr[xCord, yCord + 1] == 'U')
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        static void TurnPieces(ref char[,] changedArr, int xCord, int yCord, List<int>[] theLog, List<int>[] changed)
        {
            switch (changedArr[xCord,yCord])
            {
                case 'L':
                    changedArr[xCord, yCord] = 'U';
                    changedArr[xCord, yCord + 1] = 'U';
                    changedArr[xCord + 1, yCord] = 'D';
                    changedArr[xCord + 1, yCord + 1] = 'D';
                    break;
                case 'U':
                    changedArr[xCord, yCord] = 'L';
                    changedArr[xCord, yCord + 1] = 'R';
                    changedArr[xCord + 1, yCord] = 'L';
                    changedArr[xCord + 1, yCord + 1] = 'R';
                    break;
                default:
                    Console.WriteLine("you fucked up");
                    break;
            }
            theLog[0].Add(xCord + 1);
            theLog[1].Add(yCord + 1);
            changed[0].Add(xCord);
            changed[1].Add(yCord);
        }
    }
}