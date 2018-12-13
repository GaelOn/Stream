using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using pStream.Pipeline;

namespace StreamTest
{
    [TestFixture]
    class SharedVarTest
    {
        [Test]
        public void DataArrayShouldFlowThroughSharedVarInOrder()
        {
            var data         = new int[10];
            var dataReceived = new int[10];
            for (int i = 0; i < 10; i++)
            {
                data[i]         = i;
                dataReceived[i] = -1;
            }
            var svar = new SharedVar<int>();
            var reader = svar.GetReader();
            var writer = svar.GetWriter();

            Action readData = () =>
            {
                foreach (var item in data)
                {
                    writer.Push(item);
                }
                writer.Stop();
            };
            int id;
            var isSuccess = reader.TryRegisterReaderHandler(item => dataReceived[item] = item, out id);
            Action writeData = () => reader.Read();
            var t1 = Task.Run(readData);
            var t2 = Task.Run(writeData);
            var mre = new AutoResetEvent(false);
            Action<Task> test = (task) =>
                {
                    var because = $"all task should be complete, not {task.Status}.";
                    task.Status.Should().Be(TaskStatus.RanToCompletion, because);
                    for (int i = 0; i < 10; i++)
                    {
                        dataReceived[i].Should().Be(i);
                    }
                    mre.Set();
                };
            Action<Task> faulted = (task) =>
                {
                    Console.WriteLine(task.Exception.ToString());
                    mre.Set();
                };
            Action<Task> toSee = (task) =>
            {
                var because = $"all task should be complete, not {task.Status}.";
                task.Status.Should().Be(TaskStatus.RanToCompletion,because);
                Console.WriteLine(task.Status.ToString());
                mre.Set();
            };

            var t = Task.WhenAll(new Task[2] { t1, t2 });
            //t.ConfigureAwait(false);
            t.ContinueWith((i) => test(i), TaskContinuationOptions.NotOnFaulted);
            //t.ContinueWith((i) => faulted(i), TaskContinuationOptions.OnlyOnFaulted);
            //t.ContinueWith((i) => toSee(i));
            mre.WaitOne();
        }
    }
}