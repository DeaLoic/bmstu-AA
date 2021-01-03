using System;
using System.Collections.Generic;
using System.Text;

namespace lab_06
{
    class Graph
    {
        int[,] graph;
        public int count = 0;

        public Graph(int[,] graph)
        {
            this.graph = graph;
            count = graph.Length / 2;
        }
        public Graph(int vertexesCount, int maxPath = 10)
        {
            Random r = new Random();
            count = vertexesCount;
            graph = new int[count, count];

            for (int i = 0; i < count; i++)
            {
                graph[i, i] = -1;
                for (int j = i + 1; j < count; j++)
                {
                    int tmp = r.Next(maxPath) + 1;
                    graph[i, j] = tmp;
                    graph[j, i] = graph[i, j];
                }
            }
        }

        public int this[int i, int j]
        {
            get { return graph[i, j]; }
            set { graph[i, j] = value; }
        }
    }
}
