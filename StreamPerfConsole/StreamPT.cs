using Stream.Extension;
using System;
using System.Linq;

namespace StreamPerfConsole
{
    public static class StreamPT
    {
        public static void DoPT()
        {
            Console.ReadKey();
            var tab = new long[1000000];
            for (long it = 0; it < 1000000; it++)
            {
                tab[it] = it;
            }

            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            var expectedResult = tab.Select(x => x * 7)
                                    .Where(x => x % 2 == 0)
                                    .Average();
            sw.Stop();
            var maxExpectedTime = sw.ElapsedTicks;

            var nbElem = tab.Count(x => x % 2 == 0);

            sw.Reset();
            var stream = tab.AsStream()
                            .Map(x => x * 7)
                            .Filter(x => x % 2 == 0)
                            .Average(x => (double)x);

            sw.Start();
            var result = stream.Result();
            sw.Stop();
            var time = sw.ElapsedTicks;

            Console.WriteLine("For 1 million multiplication, 1 million euclidian division, 1 million comparison, 500k sum and one division");
            Console.WriteLine("So about 3.5 millions operations,");
            Console.WriteLine($"LINQ Result: {expectedResult}");
            Console.WriteLine($"LINQ Ticks elapsed: {maxExpectedTime}");
            Console.WriteLine($"LINQ ticks per op: {((double)maxExpectedTime) / 3500000d}");
            Console.WriteLine($"Stream Result: {result}");
            Console.WriteLine($"Stream Ticks elapsed: {time}");
            Console.WriteLine($"Stream ticks per op: {((double)time) / 3500000d}");
            Console.WriteLine($"Enhancement: {((double)(maxExpectedTime - time) * 100d) / ((double)maxExpectedTime)}");
        }
    }
}
