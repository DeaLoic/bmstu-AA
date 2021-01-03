using System;
using System.Collections.Generic;
using System.Text;

namespace lab_06
{
    static class BruteForce
    {
        public static Path GetMinPath(Graph graph)
        {
            Path shortest = new Path(graph);
            bool isStart = true;
            foreach (Path cur in GetAllRoutes(graph))
            {
                if (isStart)
                {
                    shortest = cur;
                    isStart = false;
                }
                if (shortest.N > cur.N)
                {
                    shortest = cur;
                }
            }
            return shortest;
        }

        private static List<Path> GetAllRoutes(Graph graph)
        {
            int count = graph.count;
            List<Path> routes = new List<Path>();
            List<int> data = new List<int>();
            int[] factorial = new int[count];
            int allSwap = 1;
            for (int i = 0; i < count; i++)
            {
                data.Add(i);
                allSwap *= i + 1;
                factorial[i] = allSwap;
            }

            for (int i = 0; i < allSwap; i++)
            {
                Path curRes = new Path(graph);
                List<int> source = new List<int>(data);
                for (int j = 0; j < count; j++)
                {
                    int p = i / factorial[count - 1 - j] % source.Count;
                    curRes.AddTown(source[p]);
                    source.RemoveAt(p);
                }
                curRes.AddTown(curRes.FirstTown());
                routes.Add(curRes);
            }
            return routes;
        }

        private static void Print(List<int> cur)
        {
            foreach (int num in cur)
            {
                Console.Write(num);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
