using System;
using System.Collections.Generic;
using pStream.Messages;
using pStream.WaitStrategy;
using pStream.Contract.Workers;

namespace pStream.Workers
{
    class Work<TIn, TOut> : IWork
    {
        private readonly Func<TIn, TOut> _funWork;
        private readonly Action<IMessage> _push;
        private Queue<IMessage> _queue;
        private IWaitStrategy _wait;
        private bool _shouldStop;

        public Work(Func<TIn, TOut> funWork, Queue<IMessage> readQueue, Action<IMessage> push, IWaitStrategy wait)
        {
            _queue = readQueue;
            _funWork = funWork;
            _push = push;
            _wait = wait;
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
                    switch (_queue.Dequeue())
                    {
                        case InputMessage<TIn> input:
                            IMessageValue<TOut> mess;
                            try
                            {
                                mess = new InputMessage<TOut>(_funWork(input.Value));
                            }
                            catch (Exception e)
                            {
                                mess = new ErrorMessage<TOut, Exception>(e);
                            }
                            _push(mess);
                            break;
                        case ErrorMessage<TIn, Exception> err:
                            _push(new ErrorMessage<TOut, Exception>(err.Error));
                            break;
                        case EndOfStreamMessage eos:
                            _queue = null;
                            _shouldStop = true;
                            _push(eos);
                            break;
                    }
                }
            }
            return _shouldStop;
        }
    }
}
