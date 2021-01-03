using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace lab_06
{
    class Program
    {
        const string pathBF = "./BruteForce.txt";
        const string pathAnt = "./Ant.txt";
        static void Main(string[] args)
        {
            int check = 1;
            while (check != 0)
            {
                PrintMenu();
                check = Convert.ToInt32(Console.ReadLine());
                switch (check)
                {
                    case 1:
                        Graph graph = new Graph(5, 10);
                        Path minBrutePath = BruteForce.GetMinPath(graph);
                        Path minAntPath = AntColonyOptimization.GetRoute(graph, 10, 0.6, 0.2, 4, 0.1);

                        Console.Write("Полный перебор нашел путь: ");
                        minBrutePath.Print();
                        Console.WriteLine("Длина пути: {0}\n", minBrutePath.N);

                        Console.Write("Муравьи нашли путь: ");
                        minAntPath.Print();
                        Console.WriteLine("Длина пути: {0}", minAntPath.N);
                        break;
                    case 2:
                        RunTests();
                        break;
                    case 3:
                        RunParametrized();
                        break;
                    case 0:
                        break;
                }
            }
        }

        static void PrintMenu()
        {
            Console.Write("\n0 - Выход\n1 - Демонстрация\n2 - Тестирование\n3 - Параметризация");
        }

        static void RunTests()
        {
            long time;
            Stopwatch clock = new Stopwatch();
            List<string> linesBF = new List<string>();
            List<string> linesAnt = new List<string>();

            for (int i = 2; i < 101; i++)
            {
                Graph graph = new Graph(i);
                if (i <= 10)
                {
                    clock.Restart();
                    BruteForce.GetMinPath(graph);
                    clock.Stop();
                    time = clock.ElapsedMilliseconds;
                    Console.WriteLine("BF: " + i.ToString() + " " + time.ToString());
                    linesBF.Add(i.ToString() + " " + time.ToString());
                }

                clock.Restart();
                AntColonyOptimization.GetRoute(graph, 10, 0.6, 0.2, 4, 0.1);
                clock.Stop();
                time = clock.ElapsedMilliseconds;
                Console.WriteLine("Ant: " + i.ToString() + " " + time.ToString());
                linesAnt.Add(i.ToString() + " " + time.ToString());
                if (i >= 10)
                {
                    i += 4;
                }
            }
            File.WriteAllLines(pathBF, linesBF);
            File.WriteAllLines(pathAnt, linesAnt);
        }

        static void RunParametrized()
        {
            int timeOfLive = 1;
            Console.WriteLine("TimeOfLive      alpha      beta      Q      pho      %");
            for (; timeOfLive <= 30; timeOfLive += 5)
            {
                double alpha = 0.2;
                for (; alpha <= 1; alpha += 0.4)
                {
                    double beta = 0.2;
                    for (; beta <= 1; beta += 0.4)
                    {
                        int Q = 1;
                        for (; Q <= 5; Q += 2)
                        {
                            double pho = 0.2;
                            for (; pho <= 1; pho += 0.4)
                            {
                                int correctAns = 0;
                                int series = 10;
                                for (int i = 0; i < series; i++)
                                {
                                    Graph graph = new Graph(7);
                                    int resCorrect = BruteForce.GetMinPath(graph).N;
                                    if (AntColonyOptimization.GetRoute(graph, timeOfLive, alpha, beta, Q, pho).N == resCorrect)
                                    {
                                        correctAns++;
                                    }
                                }
                                Console.WriteLine(timeOfLive.ToString() + "    " + alpha.ToString("F1") + "    " + beta.ToString("F1") + "    " + Q.ToString() + "    " +
                                                  pho.ToString("F1") + "    " + ((int)(correctAns / (double)series * 100)).ToString() + "%");
                            }
                        }
                    }
                }
                if (timeOfLive == 1)
                {
                    timeOfLive = 0;
                }
            }
        }
    }
}
