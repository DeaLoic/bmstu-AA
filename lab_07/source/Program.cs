using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace lab_07
{

    class Program
    {
        const string BASE_PATH = "F:/mare/from_win_8/Desktop/bmstu/sem_5/bmstu-AA/lab_07/source";
        const string RESULT_BINARY_PATH = BASE_PATH + "/binary.txt";
        const string RESULT_BRUTEFORCE_PATH = BASE_PATH + "/brute_force.txt";
        const string RESULT_SEGMENTS_PATH = BASE_PATH + "/segments.txt";
        const string DICTIONARY_PATH = BASE_PATH + "/dictionary.txt";

        static void Main(string[] args)
        {
            Dictionary dict = new Dictionary(DICTIONARY_PATH);
            dict.Sort();
            int check = 1;
            while (check != 0)
            {
                Console.Write("\n0 - Выход\n1 - Демонстрация\n2 - Тестирование\n");
                check = Convert.ToInt32(Console.ReadLine());
                switch (check)
                {
                    case 0:
                        break;
                    case 1:
                        Console.Write("Введите слово: ");
                        string line = Console.ReadLine().Trim();

                        int index = dict.BruteForce(line);
                        string ans = index >= 0 ? dict[index] : "Не найдено";
                        Console.Write("Brute force: " + index + "  " + ans + "\n");

                        index = dict.BinarySearch(line);
                        ans = index >= 0 ? dict[index] : "Не найдено";
                        Console.Write("Binary search: " + index + "  " + ans + "\n");

                        index = dict.FindBySegments(line);
                        ans = index >= 0 ? dict[index] : "Не найдено";
                        Console.Write("Segments: " + index + "  " + ans + "\n");
                        break;
                    case 2:
                        CheckBruteForce(dict, RESULT_BRUTEFORCE_PATH);
                        CheckBinarySearch(dict, RESULT_BINARY_PATH);
                        CheckFindBySegments(dict, RESULT_SEGMENTS_PATH);
                        break;
                    default:
                        Console.Write("Такого пункта нет, попробуйте ещё раз.");
                        break;
                }
            }
        }

        static void CheckBruteForce(Dictionary dict, string path)
        {
            Stopwatch clock = new Stopwatch();
            long time;
            int res;
            List<string> lines = new List<string>();
            Console.WriteLine("\nBrute force");
            for (int j = 0; j < dict.fill; j += 25)
            {
                time = 0;
                for (int i = 0; i < 1000; i++)
                {
                    clock.Restart();
                    res = dict.BruteForce(dict[j]);
                    clock.Stop();
                    time += clock.ElapsedTicks;
                }
                time /= 1000;
                Console.WriteLine(j.ToString() + " " + time.ToString());
                lines.Add(j.ToString() + " " + time.ToString());
            }
            File.WriteAllLines(path, lines);
        }

        static void CheckBinarySearch(Dictionary dict, string path)
        {
            Stopwatch clock = new Stopwatch();
            long time;
            int res;
            List<string> lines = new List<string>();
            Console.WriteLine("\nBinary search");
            for (int j = 0; j < dict.fill; j += 25)
            {
                time = 0;
                for (int i = 0; i < 1000; i++)
                {
                    clock.Restart();
                    res = dict.BinarySearch(dict[j]);
                    clock.Stop();
                    time += clock.ElapsedTicks;
                }
                time /= 1000;
                Console.WriteLine(j.ToString() + " " + time.ToString());
                lines.Add(j.ToString() + " " + time.ToString());
            }
            File.WriteAllLines(path, lines);
        }

        static void CheckFindBySegments(Dictionary dict, string path)
        {
            Stopwatch clock = new Stopwatch();
            long time;
            int res;
            List<string> lines = new List<string>();
            Console.WriteLine("\nSegments");
            for (int j = 0; j < dict.fill; j += 25)
            {
                time = 0;
                for (int i = 0; i < 1000; i++)
                {
                    clock.Restart();
                    res = dict.FindBySegments(dict[j]);
                    clock.Stop();
                    time += clock.ElapsedTicks;
                }
                time /= 1000;
                Console.WriteLine(j.ToString() + " " + time.ToString());
                lines.Add(j.ToString() + " " + time.ToString());
            }
            File.WriteAllLines(path, lines);
        }
    }
}
