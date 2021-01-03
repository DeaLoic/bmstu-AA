using System;
using System.Collections.Generic;
using System.Text;

namespace lab_06
{
    class Path
    {
        private List<int> path;
        public int distance;
        private Graph graph;

        public Path(Graph graph)
        {
            path = new List<int>();
            distance = 0;
            this.graph = graph;
        }

        public void AddTown(int town)
        {
            if (path.Count > 0)
            {
                distance += graph[path[path.Count - 1], town];
            }
            path.Add(town);
        }
        public int LastTown()
        {
            return path[path.Count - 1];
        }
        public int FirstTown()
        {
            return path[0];
        }
        public bool IsInPath(int town)
        {
            return path.Contains(town);
        }

        public int this[int i]
        {
            get { return path[i]; }
        }

        public int N
        {
            get { return distance; }
        }

        public void RecountDistance()
        {
            int distance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                distance += graph[path[i], path[i + 1]];
            }
            this.distance = distance;
        }
        public void Print()
        {
            foreach (int num in path)
            {
                Console.Write(num);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
