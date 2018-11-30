using System;
using System.Collections.Generic;
using System.Linq;
using Stream;
using Stream.Extension;
using pStream.Messages;
using pStream.WaitStrategy;
using System.Threading.Tasks;
using System.Threading;

namespace StreamPerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continu = true;
            while(continu)
            {
                Console.WriteLine("Choose your test:");
                Console.WriteLine(" -> perf test press '1',");
                Console.WriteLine(" -> thread test press '2',");
                Console.WriteLine(" -> quit press 'q'.");
                var k = Console.ReadKey();
                Console.WriteLine(System.Environment.NewLine);
                switch(k.KeyChar)
                {
                    case '1':
                        perfTest();
                        break;
                    case '2':
                        threadTest();
                        break;
                    case 'q':
                        continu = false;
                        break;
                }
            }
        }

        public static void threadTest()
        {

            bool flag = false;

            var qin    = new Queue<IMessage>();
            var wait   = new SleepStrategy();
            var worker = new pStream.Workers.TaskWorkStrategy<int, int>(i => i * 2, qin, wait);
            var inputs = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            // WHEN
            var outputs = new List<int>(9);
            void push()
            {
                foreach (var elem in inputs)
                {
                    qin.Enqueue(new InputMessage<int>(elem));
                }
                qin.Enqueue(new EndOfStreamMessage());
            }
            void read()
            {
                IMessage mess = null;
                var shouldEnd = false;
                while (!shouldEnd)
                {
                    if (worker.Reader.TryDequeue(out mess))
                    {
                        switch (mess)
                        {
                            case InputMessage<int> input:
                                outputs.Add(input.Value);
                                break;
                            case ErrorMessage<int, Exception> err:
                                throw err.Error;
                            case EndOfStreamMessage eos:
                                shouldEnd = true;
                                break;
                        }

                    }
                }
            }

            var tin  = Task.Factory.StartNew(() => push());
            var tout = Task.Factory.StartNew(() => read())
                           .ContinueWith((t) => flag = true);
            // THEN
            bool res = true;
            while (!Volatile.Read(ref flag))
            {
                Thread.Sleep(10);
            }
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(outputs[i]);
                res &= outputs[i] == (inputs[i] * 2);
            }
            Console.WriteLine($"Test result: {res}.");
        }

        private static void perfTest()
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

        private static Stream<int, int[]> LocalStreamProvider(int useCase)
        {
            switch (useCase)
            {
                case 0:
                    return (new int[] { 1, 2, 3 }).AsStream();
                case 1:
                    return (new int[] { 4, 5, 6 }).AsStream();
                default:
                    return (new int[] { 7, 8, 9 }).AsStream();
            }
        }
    }
}
