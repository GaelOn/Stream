using pStream.Messages;
using pStream.WaitStrategy;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamPerfConsole
{
#pragma warning disable IDE1006 // disable due to the project name tested here
    public static class pStreamPT
#pragma warning restore IDE1006 // disable due to the project name tested here
    {
        public static void ThreadTest()
        {
            bool flag = false;

            var qin = new Queue<IMessage>();
            var wait = new SleepStrategy();
            var worker = new pStream.Workers.TaskWorkerTest<int, int>(i => i * 2, qin, wait);
            var inputs = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            // WHEN
            var outputs = new List<int>(9);
            void push()
            {
                foreach (var elem in inputs)
                {
                    qin.Enqueue(new InputMessage<int>(elem));
                }
                qin.Enqueue(EndOfStreamMessage.Value);
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
                            case ErrorMessage<Exception> err:
                                throw err.Error;
                            case EndOfStreamMessage eos:
                                shouldEnd = true;
                                break;
                        }

                    }
                }
            }

            var tin = Task.Factory.StartNew(() => push());
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
    }
}
