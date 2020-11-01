using System;
using System.Diagnostics;
using System.IO;

namespace lab_04
{
    class Program
    {
        const int MAX_THREAD = 6;
        const string BASE_PATH = "F:/files/bmstu/sem_5/bmstu-AA/lab_04/";
        static void Main(string[] args)
        {
            int check = 1;
            Matrix first;
            Matrix second;
            while (check != 0)
            {
                Console.Write("0 - Выход\n1 - Демонстрация\n2 - Тестирование\n");
                check = Convert.ToInt32(Console.ReadLine());
                switch (check) {
                    case 0:
                        break;
                    case 1:
                        Console.Write("Введите размеры первой матрицы: ");
                        string[] line = Console.ReadLine().Trim().Split();
                        int n = Convert.ToInt32(line[0]);
                        int m = Convert.ToInt32(line[1]);
                        first = new Matrix(n, m);

                        Console.Write("Введите размеры второй матрицы: ");
                        line = Console.ReadLine().Trim().Split();
                        n = Convert.ToInt32(line[0]);
                        m = Convert.ToInt32(line[1]);

                        second = new Matrix(n, m);
                        first.Input();
                        second.Input();
                        Console.Write("Classic:\n");
                        VinogradMultiply.Classic(first, second).Output();
                        Console.Write("\nAll parallels:\n");
                        VinogradMultiply.ParallelFirst(first, second, 5).Output();
                        Console.Write("\nOnly main parallels:\n");
                        VinogradMultiply.ParallelSecond(first, second, 5).Output();
                        break;
                    case 2:
                        TestVariableThreads(500);
                        TestFixThreads(10, 10, 1000, MAX_THREAD);
                        break;
                    default:
                        Console.Write("Такого пункта нет, попробуйте ещё раз.");
                        break;
                }
            }
        }

        static void TestVariableThreads(int matrixSize)
        {
            Matrix first = new Matrix(matrixSize, matrixSize);
            Matrix second = new Matrix(matrixSize, matrixSize);
            first.Random();
            second.Random();

            string resultClassic = "";
            string resultParallelFirst = "";
            string resultParallelSecond = "";

            Stopwatch clock = new Stopwatch();
            for (int threads = 0; threads <= MAX_THREAD * 4; threads += 2)
            {
                int curThreads = threads > 0 ? threads : 1;

                long timeClassic = 0;
                long timeFirstParallel = 0;
                long timeSecondParallel = 0;

                for (int i = 0; i < 4; i++)
                {
                    clock.Restart();
                    VinogradMultiply.Classic(first, second);
                    clock.Stop();
                    timeClassic += clock.ElapsedMilliseconds;

                    clock.Restart();
                    VinogradMultiply.ParallelFirst(first, second, curThreads);
                    clock.Stop();
                    timeFirstParallel += clock.ElapsedMilliseconds;


                    clock.Restart();
                    VinogradMultiply.ParallelSecond(first, second, curThreads);
                    clock.Stop();
                    timeSecondParallel += clock.ElapsedMilliseconds;
                }
                timeClassic /= 4;
                timeFirstParallel /= 4;
                timeSecondParallel /= 4;

                resultClassic += curThreads.ToString() + " " + timeClassic.ToString() + "\n";
                resultParallelFirst += curThreads.ToString() + " " + timeFirstParallel.ToString() + "\n";
                resultParallelSecond += curThreads.ToString() + " " + timeSecondParallel.ToString() + "\n";
                Console.WriteLine(curThreads.ToString() + "\n" + timeClassic.ToString() + " " +
                                  timeFirstParallel.ToString() + " " + timeSecondParallel.ToString() + "\n");
            }
            File.WriteAllText(BASE_PATH + "testClassicMulty.txt", resultClassic);
            File.WriteAllText(BASE_PATH + "testParallelFirstMulty.txt", resultParallelFirst);
            File.WriteAllText(BASE_PATH + "testParallelSecondMulty.txt", resultParallelSecond);
        }

        static void TestFixThreads(int matrixSizeStart, int matrixSizeCount, int matrixSizeEnd, int threads)
        {
            threads = threads > 0 ? threads : 1;
            matrixSizeCount = matrixSizeCount > 0 ? matrixSizeCount : 1;
            int step = (matrixSizeEnd - matrixSizeStart) / matrixSizeCount;

            string resultClassic = "";
            string resultParallelFirst = "";
            string resultParallelSecond = "";

            long timeClassic = 0;
            long timeFirstParallel = 0;
            long timeSecondParallel = 0;
            for (int size = matrixSizeStart; size <= matrixSizeEnd; size += step)
            {
                Matrix first = new Matrix(size, size);
                Matrix second = new Matrix(size, size);
                first.Random();
                second.Random();

                Stopwatch clock = new Stopwatch();
                for (int i = 0; i < 4; i++)
                {
                    clock.Restart();
                    VinogradMultiply.Classic(first, second);
                    clock.Stop();
                    timeClassic += clock.ElapsedMilliseconds;

                    clock.Restart();
                    VinogradMultiply.ParallelFirst(first, second, threads);
                    clock.Stop();
                    timeFirstParallel += clock.ElapsedMilliseconds;


                    clock.Restart();
                    VinogradMultiply.ParallelSecond(first, second, threads);
                    clock.Stop();
                    timeSecondParallel += clock.ElapsedMilliseconds;
                }
                timeClassic /= 4;
                timeFirstParallel /= 4;
                timeSecondParallel /= 4;

                resultClassic += size.ToString() + " " + timeClassic.ToString() + "\n";
                resultParallelFirst += size.ToString() + " " + timeFirstParallel.ToString() + "\n";
                resultParallelSecond += size.ToString() + " " + timeSecondParallel.ToString() + "\n";
                Console.WriteLine(size.ToString() + "\n" + timeClassic.ToString() + " " +
                                  timeFirstParallel.ToString() + " " + timeSecondParallel.ToString() + "\n");
            }
            File.WriteAllText(BASE_PATH + "testClassicFix.txt", resultClassic);
            File.WriteAllText(BASE_PATH + "testParallelFirstFix.txt", resultParallelFirst);
            File.WriteAllText(BASE_PATH + "testParallelSecondFix.txt", resultParallelSecond);
        }
    }
}
