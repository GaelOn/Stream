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
        [TestCase(0,0)]
        [TestCase(50,0)]
        [TestCase(0,50)]
        public void DataArrayShouldFlowThroughSharedVarInOrder(int timeToWaitWriter, int timeToWaitReader)
        {
            // GIVEN
            var data = new int[10];
            var dataReceived = new int[10];
            for (int i = 0; i < 10; i++)
            {
                data[i]         = i;
                dataReceived[i] = -1;
            }
            var svar = new SharedVar<int>();
            var reader = svar.GetReader();
            var writer = svar.GetWriter();

            int id;
            var isSuccess = reader.TryRegisterReaderHandler(GetReader(timeToWaitReader, dataReceived), out id);
            Action readData = () => reader.Read();
            // WHEN
            var t1 = Task.Factory.StartNew(GetWriter(timeToWaitWriter, data, writer));
            var t2 = Task.Factory.StartNew(readData);
            var mre = new AutoResetEvent(false);
            void testFailureAndSet(Task task)
            {
                var because = $"all task should be complete, not {task.Status}.";
                task.Status.Should().Be(TaskStatus.RanToCompletion, because);                
                mre.Set();
            }
            
            var t = Task.WhenAll(new Task[2] { t1, t2 });
            t.ConfigureAwait(false);
            // THEN
            t.ContinueWith((i) => testFailureAndSet(i), TaskContinuationOptions.NotOnFaulted);
            mre.WaitOne();
            for (int i = 0; i < 10; i++)
            {
                dataReceived[i].Should().Be(i);
            }
        }

        #region Private helper method
        private Action GetWriter(int timeWait, int[] data, IWriter<int> writer)
        {
            switch (timeWait)
            {
                case 0:
                    return () =>
                    {
                        foreach (var item in data)
                        {
                            writer.Push(item);
                        }
                        writer.Stop();
                    };
                default:
                    return () =>
                    {
                        foreach (var item in data)
                        {
                            writer.Push(item);
                            Thread.Sleep(timeWait);
                        }
                        writer.Stop();
                    };
            }
        }

        private Action<int> GetReader(int timeWait, int[] data)
        {
            switch (timeWait)
            {
                case 0:
                    return (item) => data[item] = item;
                default:
                    return (item) =>
                    {
                        data[item] = item;
                        Thread.Sleep(timeWait);
                    };
            }
        } 
        #endregion
    }
}