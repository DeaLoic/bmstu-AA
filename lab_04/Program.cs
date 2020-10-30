using System;

namespace lab_04
{
    class Program
    {
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
                        first = new Matrix(3, 3);
                        second = new Matrix(3, 3);
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
                        break;
                    default:
                        Console.Write("Такого пункта нет, попробуйте ещё раз.");
                        break;
                }
            }
        }
    }
}
