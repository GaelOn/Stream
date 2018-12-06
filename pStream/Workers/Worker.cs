using System;
using System.Collections.Generic;
using pStream.Messages;
using pStream.WaitStrategy;
using System.Threading;

namespace pStream.Workers
{
    internal class Worker<TIn, TOut> : IWorker<TIn>, IDisposable
    {
        private readonly Func<TIn, TOut> _funWork;
        private readonly Action<IMessage> _push;
        private Queue<IMessage> _queue;
        private readonly IMessageVisitor _msgVisitor;
        private IWaitStrategy _wait;
        private bool _shouldStop;

        public event OnEndOfStreamHandler OnEndOfStream;

        public void RaiseOnEndOfStream() => Volatile.Read(ref OnEndOfStream)?.Invoke(this, new  EndOfStreamEventArg(EndOfStreamMessage.Value));

        public void Push(IMessage msg) => _push(msg);

        public IMessage DoWork(TIn entry) => new InputMessage<TOut>(_funWork(entry));

        public Worker(IMessageVisitorFactory msgVisitorFactory, IWaitStrategy wait, Func<TIn, TOut> funWork, Queue<IMessage> readQueue, Action<IMessage> push)
        {
            _queue         = readQueue;
            _funWork       = funWork;
            _push          = push;
            _wait          = wait;
            _msgVisitor    = msgVisitorFactory.Create(this);
            OnEndOfStream += OnEndOfStreamHandler;
        }

        public void Start() => _wait?.Run(Read);

        public bool Read()
        {
            if (_queue == null)
            {
                return true;
            }
            if (!_shouldStop)
            {
                while (_queue.Count > 0)
                {
                    _queue.Dequeue().Accept(_msgVisitor);
                }
            }
            return _shouldStop;
        }

        private void OnEndOfStreamHandler(object sender, EndOfStreamEventArg arg)
        {
            _queue      = null;
            _shouldStop = true;
            _push(arg.EOS);
        }

        public void Dispose() => OnEndOfStream -= OnEndOfStreamHandler;
    }
}
