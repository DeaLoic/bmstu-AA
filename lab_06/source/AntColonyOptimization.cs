using System;
using System.Collections.Generic;
using System.Text;

namespace lab_06
{
    class Ant
    {
        private Path path;

        public Ant(Graph graph)
        {
            path = new Path(graph);
        }
        public void VisitTown(int town)
        {
            path.AddTown(town);
        }
        public int LastVisited()
        {
            return path.LastTown();
        }
        public bool IsVisited(int town)
        {
            return path.IsInPath(town);
        }
        public int GetDistance()
        {
            return path.N;
        }
        public Path GetPath()
        {
            return path;
        }
        public int Start
        {
            get { return path.FirstTown(); }
        }
        public void Print()
        {
            path.Print();
        }
    }
    static class AntColonyOptimization
    {
        public static Path GetRoute(Graph graph, int maxTime, double alpha, double beta, double Q, double pho)
        {
            Random r = new Random();

            Path shortest = new Path(graph);
            shortest.distance = int.MaxValue;

            int count = graph.count;
            double[,] pher = InitPheromone(0.1, count);

            for (int time = 0; time < maxTime; time++)
            {
                List<Ant> ants = InitAnts(graph);
                for (int i = 0; i < count - 1; i++)
                {
                    double[,] deltaPher = InitPheromone(0, count);
                    foreach (Ant ant in ants)
                    {
                        int curTown = ant.LastVisited();

                        double sum = 0;
                        for (int town = 0; town < count; town++)
                        {
                            if (!ant.IsVisited(town))
                            {
                                double tau = pher[curTown, town];
                                double eta = 1.0 / graph[curTown, town];
                                sum += Math.Pow(tau, alpha) * Math.Pow(eta, beta);
                            }
                        }

                        double[] probability = new double[count];
                        for (int checkTown = 0; checkTown < count; checkTown++)
                        {
                            double chance = 0;
                            if (!ant.IsVisited(checkTown))
                            {
                                double tau = pher[curTown, checkTown];
                                double eta = 1.0 / graph[curTown, checkTown];
                                chance = Math.Pow(tau, alpha) * Math.Pow(eta, beta) / sum;
                            }
                            probability[checkTown] = chance;
                        }
                        int newTown = GetPosByProb(probability);
                        ant.VisitTown(newTown);
                        deltaPher[curTown, newTown] += Q / ant.GetDistance();
                    }
                    for (int k = 0; k < count; k++)
                    {
                        for (int t = 0; t < count; t++)
                        {
                            pher[k, t] = (1 - pho) * pher[k, t] + deltaPher[k, t];
                            if (pher[k, t] < 0.1)
                            {
                                pher[k, t] = 0.1;
                            }
                        }
                    }
                }
                foreach (Ant ant in ants)
                {
                    ant.VisitTown(ant.Start);

                    if (ant.GetDistance() < shortest.N)
                    {
                        shortest = ant.GetPath();
                    }
                }
            }
            return shortest;
        }
        static int GetPosByProb(double[] probability)
        {
            double[] comulativeProb = new double[probability.Length];
            comulativeProb[0] = probability[0];
            for (int i = 1; i < probability.Length; i++)
            {
                comulativeProb[i] = probability[i] + comulativeProb[i - 1];
            }
            
            Random random = new Random((int)DateTime.UtcNow.Ticks);
            int res = 0;
            double choose = random.NextDouble();
            for (int i = 0; i < comulativeProb.Length; i++)
            {
                if (choose <= comulativeProb[i] && probability[i] != 0)
                {
                    res = i;
                    break;
                }
            }
            return res;

        }

        private static List<Ant> InitAnts(Graph graph)
        {
            List<Ant> ants = new List<Ant>();
            for (int i = 0; i < graph.count; i++)
            {
                ants.Add(new Ant(graph));
                ants[i].VisitTown(i);
            }
            return ants;
        }

        private static double[,] InitPheromone(double num, int size)
        {
            double[,] phen = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    phen[i, j] = num;
                }
            }
            return phen;
        }
    }
}
