using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using pStream.Messages;
using pStream.WaitStrategy;
using pStream.Workers;

namespace StreamTest 
{
    [TestFixture]
    public class WorkerTest 
    {
        [Test]
        public void TaskWorkStrategyTest () 
        {
            // GIVEN
            var qin    = new Queue<IMessage>();
            var wait   = new SleepStrategy();
            var worker = new TaskWorkStrategy<int, int>(i => i*2, qin, wait);
            var inputs = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	        var are    = new AutoResetEvent (false);
            // WHEN
            var outputs = new List<int> (9);
            void push ()
            {
                foreach (var elem in inputs) 
                {
                    qin.Enqueue (new InputMessage<int> (elem));
                }
                qin.Enqueue(new EndOfStreamMessage());
            }   
            void read ()
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
                                outputs.Add (input.Value);
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

            var tin  = Task.Factory.StartNew(push);
            var tout = Task.Factory.StartNew(read)
			   .ContinueWith((t) => are.Set());
            // THEN
	        are.WaitOne ();
            for (int i = 0; i < 9; i++) 
            {
		        outputs[i].Should().Be(inputs[i] * 2);
            }
        }
    }
}