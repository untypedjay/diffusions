using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TPLIntegration
{
    class Program
    {
        static double F(double x)
        {
            return 4.0 / (1 + x * x);
        }

        static double IntegrateSeq(int n, double lb, double ub)
        {
            double sum = 0.0;
            double w = (ub - lb) / n;

            for (int i = 0; i < n; i++)
            {
                sum += w * F(lb + w * (i + 0.5));
            }
            return sum;
        }

        static double IntegratePar(int n, double lb, double ub)
        {
            object locker = new object();
            double sum = 0.0;
            double w = (ub - lb) / n;

            var partitioner = Partitioner.Create(0, n);

            Parallel.ForEach(partitioner,
                () => 0.0,
                (range, pls, partialSum) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        partialSum += w * F(lb + w * (i + 0.5));
                    }
                    
                    return partialSum;
                },
                partialSum =>
                {
                    lock (locker)
                    {
                        sum += partialSum;
                    }
                }
            );

            return sum;
        }

        static void Main(string[] args)
        {
            int n;
            Console.Write("n > ");
            n = int.Parse(Console.ReadLine());
            Console.WriteLine();

            while (n < 1000000000)
            {
                var sw = new Stopwatch();
                sw.Start();
                double resSeq = IntegrateSeq(n, 0.00, 1.0);
                sw.Stop();
                double timeSeq = sw.ElapsedMilliseconds;

                sw.Restart();
                double resPar = IntegratePar(n, 0.00, 1.0);
                sw.Stop();
                double timePar = sw.ElapsedMilliseconds;

                Console.WriteLine($"n: {n}");
                Console.WriteLine($"Result: {resSeq} (seq) \t {resPar} (TPL)");
                Console.WriteLine($"Time: {timeSeq} ms (seq) \t {timePar} ms (TPL)");
                Console.WriteLine($"Speedup: {timeSeq / timePar}");
                Console.WriteLine();

                n = n * 2;
            }
        }
    }
}
