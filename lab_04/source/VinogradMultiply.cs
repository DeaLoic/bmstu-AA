using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab_04
{
    static class VinogradMultiply
    {
            public static Matrix Classic(Matrix first, Matrix second)
            {
                Matrix result = new Matrix(0, 0);
                if (first.M == second.N && first.N != 0 && second.M != 0)
                {
                    int n1 = first.N;
                    int m1 = first.M;
                    int n2 = second.N;
                    int m2 = second.M;

                    result = new Matrix(n1, m2);
                    int[] mulH = new int[n1];
                    int[] mulV = new int[m2];

                    for (int i = 0; i < n1; i++)
                    {
                        for (int j = 0; j < m1 / 2; j++)
                        {
                            mulH[i] += first[i, j * 2] * first[i, j * 2 + 1];
                        }
                    }

                    for (int i = 0; i < m2; i++)
                    {
                        for (int j = 0; j < n2 / 2; j++)
                        {
                            mulV[i] += second[j * 2, i] * second[j * 2 + 1, i];
                        }
                    }

                    for (int i = 0; i < n1; i++)
                    {
                        for (int j = 0; j < m2; j++)
                        {
                            result[i, j] = -mulH[i] - mulV[j];
                            for (int k = 0; k < m1 / 2; k++)
                            {
                                result[i, j] += (first[i, 2 * k + 1] + second[2 * k, j]) * (first[i, 2 * k] + second[2 * k + 1, j]);
                            }
                        }
                    }

                    if (m1 % 2 == 1)
                    {
                        for (int i = 0; i < n1; i++)
                        {
                            for (int j = 0; j < m2; j++)
                            {
                                result[i, j] += first[i, m1 - 1] * second[m1 - 1, j];
                            }
                        }
                    }
                }

                return result;
            }

        // параллелится и mulh и mulv одновременно, потом паралл на главную и параллел на последнюю
        public static Matrix ParallelFirst(Matrix first, Matrix second, int threadsCount)
        {
            Matrix result = new Matrix(0, 0);
            if (threadsCount <= 1)
            {
                result = Classic(first, second);
            }
            else if (first.M == second.N && first.N != 0 && second.M != 0)
            {
                int n1 = first.N;
                int m1 = first.M;
                int n2 = second.N;
                int m2 = second.M;

                result = new Matrix(n1, m2);
                int[] mulH = new int[n1];
                int[] mulV = new int[m2];

                Thread[] threads = new Thread[threadsCount];

                int threadsHalfing = threadsCount / 2;

                // mulH
                int threadWork = threadsHalfing;
                int step = n1 / threadWork;
                if (threadWork / n1 >= 1)
                {
                    step = 1;
                    threadWork = n1;
                    threadsHalfing = threadWork;
                }
                int start = 0;
                for (int i = 0; i < threadWork - 1; i++)
                {
                    threads[i] = new Thread(CountMulH);
                    threads[i].Start(new ParametersForMul(first, mulH, start, start + step, m1));
                    start += step;
                }
                threads[threadWork - 1] = new Thread(CountMulH);
                threads[threadWork - 1].Start(new ParametersForMul(first, mulH, start, n1, m1));

                // MulV
                threadWork = (threadsCount - threadsHalfing);
                step = m2 / threadWork;
                if (threadWork / m2 >= 1)
                {
                    step = 1;
                    threadWork = m2;
                }
                start = 0;
                for (int i = threadsHalfing; i < threadsHalfing + threadWork - 1; i++)
                {
                    threads[i] = new Thread(CountMulV);
                    threads[i].Start(new ParametersForMul(second, mulV, start, start + step, n2));
                    start += step;
                }
                threads[threadsHalfing + threadWork - 1] = new Thread(CountMulV);
                threads[threadsHalfing + threadWork - 1].Start(new ParametersForMul(second, mulV, start, m2, n2));

                //sync
                for (int i = 0; i < threadWork + threadsHalfing; i++)
                {
                    threads[i].Join();
                }

                //Main
                step = n1 / threadsCount;
                threadWork = threadsCount;
                if (threadsCount / n1 >= 1)
                {
                    step = 1;
                    threadWork = n1;
                }
                start = 0;
                for (int i = 0; i < threadWork - 1; i++)
                {
                    threads[i] = new Thread(CountMain);
                    threads[i].Start(new ParametersForMain(result, first, second, mulV, mulH, start, start + step, m2, m1));
                    start += step;
                }
                threads[threadWork - 1] = new Thread(CountMain);
                threads[threadWork - 1].Start(new ParametersForMain(result, first, second, mulV, mulH, start, n1, m2, m1));

                // sync
                for (int i = 0; i < threadWork; i++)
                {
                    threads[i].Join();
                }

                // end
                if (m1 % 2 == 1)
                {
                    start = 0;
                    for (int i = 0; i < threadWork - 1; i++)
                    {
                        threads[i] = new Thread(CountTail);
                        threads[i].Start(new ParametersForMain(result, first, second, mulV, mulH, start, start + step, m2, m1));
                        start += step;
                    }
                    threads[threadWork - 1] = new Thread(CountTail);
                    threads[threadWork - 1].Start(new ParametersForMain(result, first, second, mulV, mulH, start, n1, m2, m1));

                    // sync
                    for (int i = 0; i < threadWork; i++)
                    {
                        threads[i].Join();
                    }
                }
            }

            return result;
        }

        // параллелится только main
        public static Matrix ParallelSecond(Matrix first, Matrix second, int threadsCount)
        {
            Matrix result = new Matrix(0, 0);
            if (threadsCount <= 1)
            {
                result = Classic(first, second);
            }
            else if (first.M == second.N && first.N != 0 && second.M != 0)
            {
                int n1 = first.N;
                int m1 = first.M;
                int n2 = second.N;
                int m2 = second.M;

                result = new Matrix(n1, m2);
                int[] mulH = new int[n1];
                int[] mulV = new int[m2];

                for (int i = 0; i < n1; i++)
                {
                    for (int j = 0; j < m1 / 2; j++)
                    {
                        mulH[i] += first[i, j * 2] * first[i, j * 2 + 1];
                    }
                }

                for (int i = 0; i < m2; i++)
                {
                    for (int j = 0; j < n2 / 2; j++)
                    {
                        mulV[i] += second[j * 2, i] * second[j * 2 + 1, i];
                    }
                }

                //Main
                Thread[] threads = new Thread[threadsCount];
                int step = n1 / threadsCount;
                int threadWork = threadsCount;
                if (threadsCount / n1 >= 1)
                {
                    step = 1;
                    threadWork = n1;
                }
                int start = 0;
                for (int i = 0; i < threadWork - 1; i++)
                {
                    threads[i] = new Thread(CountMain);
                    threads[i].Start(new ParametersForMain(result, first, second, mulV, mulH, start, start + step, m2, m1));
                    start += step;
                }
                threads[threadWork - 1] = new Thread(CountMain);
                threads[threadWork - 1].Start(new ParametersForMain(result, first, second, mulV, mulH, start, n1, m2, m1));

                // sync
                for (int i = 0; i < threadWork; i++)
                {
                    threads[i].Join();
                }

                // end
                if (m1 % 2 == 1)
                {
                    for (int i = 0; i < n1; i++)
                    {
                        for (int j = 0; j < m2; j++)
                        {
                            result[i, j] += first[i, m1 - 1] * second[m1 - 1, j];
                        }
                    }
                }
            }

            return result;
        }

        private static void CountMulH(object paramsObj)
        {
            ParametersForMul paramses = (ParametersForMul)paramsObj;
            int start = paramses.start,
                end = paramses.end,
                secondBorder = paramses.secondBorder;
            Matrix matrix = paramses.matrix;
            int[] mul = paramses.array;

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < secondBorder / 2; j++)
                {
                    mul[i] += matrix[i, j * 2] * matrix[i, j * 2 + 1];
                }
            }
        }

        private static void CountMulV(object paramsObj)
        {
            ParametersForMul paramses = (ParametersForMul)paramsObj;
            int start = paramses.start,
                end = paramses.end,
                secondBorder = paramses.secondBorder;
            Matrix matrix = paramses.matrix;
            int[] mul = paramses.array;

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < secondBorder / 2; j++)
                {
                    mul[i] += matrix[j * 2, i] * matrix[j * 2 + 1, i];
                }
            }
        }

        private static void CountMain(object paramsObj)
        {
            ParametersForMain paramses = (ParametersForMain)paramsObj;
            Matrix result = paramses.result,
                first = paramses.first,
                second = paramses.second;
            int[] mulH = paramses.mulH,
                mulV = paramses.mulV;
            int start = paramses.start,
                end = paramses.end,
                m2 = paramses.m2,
                m1 = paramses.m1;

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < m2; j++)
                {
                    result[i, j] = -mulH[i] - mulV[j];
                    for (int k = 0; k < m1 / 2; k++)
                    {
                        result[i, j] += (first[i, 2 * k + 1] + second[2 * k, j]) * (first[i, 2 * k] + second[2 * k + 1, j]);
                    }
                }
            }
        }

        private static void CountTail(object paramsObj)
        {
            ParametersForMain paramses = (ParametersForMain)paramsObj;
            Matrix result = paramses.result,
                first = paramses.first,
                second = paramses.second;
            int start = paramses.start,
                end = paramses.end,
                m2 = paramses.m2,
                m1 = paramses.m1;

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < m2; j++)
                {
                    result[i, j] += first[i, m1 - 1] * second[m1 - 1, j];
                }
            }
        }
    }

    class ParametersForMul
    {
        public Matrix matrix;
        public int[] array;
        public int start, end, secondBorder;

        public ParametersForMul(Matrix matrix, int[] array, int start, int end, int secondBorder)
        {
            this.matrix = matrix;
            this.array = array;
            this.start = start;
            this.end = end;
            this.secondBorder = secondBorder;
        }
    }

    class ParametersForMain
    {
        public Matrix first, second, result;
        public int[] mulH, mulV;
        public int start, end, m2, m1;

        public ParametersForMain(Matrix result, Matrix first, Matrix second,
                                 int[] mulV, int[] mulH,
                                 int start, int end, int m2, int m1)
        {
            this.first = first;
            this.second = second;
            this.result = result;
            this.mulH = mulH;
            this.mulV = mulV;
            this.start = start;
            this.end = end;
            this.m1 = m1;
            this.m2 = m2;
        }
    }
}