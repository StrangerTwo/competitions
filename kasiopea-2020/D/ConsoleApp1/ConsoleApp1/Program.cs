using System;
using System.Collections.Generic;
using System.IO;

namespace B
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("D.txt"))
            {
                using (StreamWriter w = new StreamWriter("D-vysledek.txt"))
                {
                    int T = int.Parse(r.ReadLine());
                    for (int t = 0; t < T; t++)
                    {

                        string line = r.ReadLine();
                        string[] tokens = line.Split(' ');
                        int countOfStudents = Convert.ToInt32(tokens[0]);
                        int countOfRelationships = Convert.ToInt32(tokens[1]);
                        bool isItPossible = true;
                        string odpoved = "ANO";

                        List<int>[] data = new List<int>[countOfStudents];
                        for (int i = 0; i < countOfStudents; i++)
                        {
                            data[i] = new List<int>();
                        }


                        for (int i = 0; i < countOfRelationships; i++)
                        {
                            line = r.ReadLine();
                            tokens = line.Split(' ');

                            int student = Convert.ToInt32(tokens[0])-1;
                            int friend = Convert.ToInt32(tokens[1])-1;

                            data[student].Add(friend);
                            data[friend].Add(student);
                        }
                        foreach (List<int> item in data)
                        {
                            item.Sort();
                            if (item.Count == 0)
                            {
                                isItPossible = false;
                                odpoved = "NE";
                            }
                        }

                        w.WriteLine(odpoved);
                        if (isItPossible == true)
                        {
                            int numberOfRulers = 0;
                            int[] toolArr = new int[countOfStudents];      // 0 - neurčeno; 1 - pravítko; 2 - kružítko
                            bool finished = false;
                            bool doplniliJsmeNeco = false;
                            bool doplnJakekoliCislo = false;
                            int pokus = 0;
                            int obsazenoNastroju = 0; 

                            for (int i = 0; i < countOfStudents; i++)
                            {
                                if (toolArr[i] == 0)
                                {
                                    //Pokud mám pouze 1 kamaráda, vezmu si pravítko, pokud není potřeba jinak
                                    if (data[i].Count == 1)
                                    {
                                        if (toolArr[data[i][0]] == 0 || toolArr[data[i][0]] == 2)
                                        {
                                            toolArr[i] = 1;
                                            toolArr[data[i][0]] = 2;
                                        }
                                        else
                                        {
                                            toolArr[i] = 2;
                                            toolArr[data[i][0]] = 1;
                                        }
                                    }
                                }
                            }

                            Console.Clear();
                            Console.WriteLine("Příklad {0} z {1}", t, T);
                            Console.WriteLine("Hotovo : {0} z {1} ({2} %)", obsazenoNastroju, countOfStudents, obsazenoNastroju * 100 / countOfStudents);

                            do
                            {
                                doplniliJsmeNeco = false;
                                pokus++;

                                //Budeme se pokoušet doplnit každého studenta
                                for (int i = 0; i < countOfStudents; i++)
                                {
                                    if (toolArr[i] == 0)
                                    {
                                        int needToHave = 0;                       // 0 - Neurčeno; 1 - pravítko; 2 - kružítko; 3 - pokaženo

                                        //Pokud všichni tvoji kamarádi mají pravítka, musíš mít kružítko + opak
                                        foreach (int kamarad in data[i])
                                        {
                                            if (toolArr[kamarad] == 1)
                                            {
                                                if (needToHave == 1)
                                                {
                                                    needToHave = 3;
                                                }
                                                else
                                                {
                                                    needToHave = 2;
                                                }
                                            }
                                            if (toolArr[kamarad] == 2)
                                            {
                                                if (needToHave == 2)
                                                {
                                                    needToHave = 3;
                                                }
                                                else
                                                {
                                                    needToHave = 1;
                                                }
                                            }
                                        }
                                        if (needToHave == 1 || needToHave == 2)
                                        {
                                            toolArr[i] = needToHave;
                                            doplniliJsmeNeco = true;
                                        }
                                        else if (needToHave == 3)
                                        {
                                            toolArr[i] = 1;
                                            doplniliJsmeNeco = true;
                                        }
                                        else if (needToHave == 0 && doplnJakekoliCislo)
                                        {
                                            toolArr[i] = 1;
                                            doplniliJsmeNeco = true;
                                            doplnJakekoliCislo = false;
                                        }
                                    }
                                }

                                if (doplniliJsmeNeco == false)
                                {
                                    doplnJakekoliCislo = true;
                                }

                                obsazenoNastroju = 0;
                                foreach (int item in toolArr)
                                {
                                    if (item != 0)
                                    {
                                        obsazenoNastroju++;
                                    }
                                }
                                if (obsazenoNastroju == countOfStudents)
                                {
                                    finished = true;
                                }

                                if (pokus % 1500 == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Příklad {0} z {1}", t, T);
                                    Console.WriteLine("Hotovo : {0} z {1} ({2} %)",obsazenoNastroju,countOfStudents,obsazenoNastroju * 100 / countOfStudents);
                                }

                            } while (!finished);

                            //KONTROLA
                            for (int i = 0; i < countOfStudents; i++)
                            {
                                bool prosel = false;
                                int studentuvNastroj = toolArr[i];
                                foreach (int kamarad in data[i])
                                {
                                    int kamaraduvNastroj = toolArr[kamarad];
                                    if (studentuvNastroj == 1 && kamaraduvNastroj == 2 || studentuvNastroj == 2 && kamaraduvNastroj == 1)
                                    {
                                        prosel = true;
                                    }
                                }
                                if (prosel)
                                {

                                }
                                else
                                {
                                    Console.WriteLine("explain oks?");
                                    Console.Write("Student {0} má {1}; kamarádi : ",i,studentuvNastroj);
                                    foreach (int kamarad in data[i])
                                    {
                                        int kamaraduvNastroj = toolArr[kamarad];
                                        Console.Write("{0}; ",kamaraduvNastroj);
                                    }
                                    Console.WriteLine();
                                }
                            }


                            Console.Clear();
                            Console.WriteLine("Příklad {0} z {1}", t, T);
                            Console.WriteLine("Hotovo : {0} z {1} ({2} %)", obsazenoNastroju, countOfStudents, obsazenoNastroju * 100 / countOfStudents);

                            foreach (int item in toolArr)
                            {
                                if (item == 1)
                                {
                                    numberOfRulers++;
                                }
                            }

                            w.WriteLine(numberOfRulers);
                            for (int i = 0; i < countOfStudents; i++)
                            {
                                if (toolArr[i] == 1)
                                {
                                    w.WriteLine(i+1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}