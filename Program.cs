using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {
        public static int ParallelKnapsack(int W, int[] wt, int[] pr, int n, int nThreads)
        {
            int[,] K = new int[n + 1, W + 1];
            var watch = Stopwatch.StartNew();
            watch.Start();
            Parallel.For(0, n + 1, new ParallelOptions { MaxDegreeOfParallelism = nThreads }, i =>
            {
                for (int w = 0; w <= W; w++)
                {
                    if (i == 0 || w == 0)
                        K[i, w] = 0;
                    else if (wt[i - 1] <= w)
                        K[i, w] = Math.Max(pr[i - 1] + K[i - 1, w - wt[i - 1]], K[i - 1, w]);
                    else
                        K[i, w] = K[i - 1, w];
                }
            });
            watch.Stop();
            Console.WriteLine("Execution Time: " + watch.ElapsedMilliseconds + "ms");
            return K[n, W];
        }
        static void Main(string[] args)
        {
            int n = 100000;
            Console.Write("Enter number of threads: ");
            int nThr = Convert.ToInt32(Console.ReadLine());
            Random rand = new Random();
            int[] prices = Enumerable.Repeat(0, n).Select(i => rand.Next(0, 1000)).ToArray();
            int[] weights = Enumerable.Repeat(0, n).Select(i => rand.Next(0, 1000)).ToArray();
            int W = 1500;

            Console.WriteLine(ParallelKnapsack(W, weights, prices, prices.Length, nThr));
        }
    }
}