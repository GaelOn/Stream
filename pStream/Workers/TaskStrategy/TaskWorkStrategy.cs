using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using pStream.Contract.Workers;
using pStream.Messages;
using pStream.WaitStrategy;

namespace pStream.Workers 
{
    internal sealed class TaskWorkStrategy<TIn, TOut> : IWorkStrategy
    {
        private Task _task;
        private Queue<IMessage> _qin;
        private ConcurrentQueue<IMessage> _qout;

        public TaskWorkStrategy(Func<TIn, TOut> funWork, Queue<IMessage> qin, IWaitStrategy wait, CancellationToken ct = default)
        {
            _qout = new ConcurrentQueue<IMessage>();
            var _wait = wait ?? new SleepStrategy();
            var worker = new Work<TIn, TOut>(funWork, qin, _qout.Enqueue, _wait);
            ct = ct == null ? CancellationToken.None : ct;
            _task = Task.Factory.StartNew(() => worker.Start(), ct);
        }

        public void Push(IMessage input)
        {
            _qin?.Enqueue(input);
        }

        public ConcurrentQueue<IMessage> Reader { get { return _qout; } }

        public void Dispose () 
        {
            _qin = null;
            _qout = null;
        }
    }
}