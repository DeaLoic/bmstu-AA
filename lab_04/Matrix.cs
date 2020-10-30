using System;
using System.Collections.Generic;
using System.Text;

namespace lab_04
{
    class Matrix
    {
        private int n = 0;
        private int m = 0;
        private int[,] body;

        public Matrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            body = new int[n, m];
        }

        public int N => n;
        public int M => m;

        public int this[int i, int j]
        {
            get => body[i, j];
            set => body[i, j] = value;
        }

        public void Input()
        {
            Console.WriteLine("Введите матрицу размером {0}x{1}", N, M);
            for (int i = 0; i < N; i++)
            {
                string[] value = Console.ReadLine().Trim().Split();
                for (int j = 0; j < value.Length; j++)
                {
                    body[i, j] = Convert.ToInt32(value[j]);
                }
            }
        }

        public void Output()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write("{0, 7}", body[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void Random()
        {
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    body[i, j] = random.Next(100);
                }
            }
        }
    }
}
