using System;
using pStream.Helper;
using System.Threading;
using pStream.Messages;
using pStream.Pipeline;

namespace pStream.Workers
{
    internal sealed class Worker<TIn, TOut> : IWorker<TIn>, IPipeable<IMessage>, IReadable<IMessage>
    {
        #region Private variables
        private readonly Func<TIn, TOut>   _funWork;
        private IReadable<IMessage>        _fromReader;
        private readonly IReader<IMessage> _toReader;
        private readonly IWriter<IMessage> _writer;
        private readonly IMessageVisitor   _msgVisitor;
//        private readonly bool              _shouldStopOnError;
        private readonly IUnsubscriber     _unsubscriber;
        #endregion  

        #region IWorker<TIn> implementation
        public event OnEndOfStreamHandler OnEndOfStream;

        public void RaiseOnEndOfStream() => Volatile.Read(ref OnEndOfStream)?.Invoke(this, new EndOfStreamEventArg(EndOfStreamMessage.Value));

        public void Push(IMessage msg) => _writer.Push(msg);

        public IMessage DoWork(TIn entry) => new InputMessage<TOut>(_funWork(entry));

        public Worker(IMessageVisitorFactory msgVisitorFactory, Func<TIn, TOut> funWork, Func<ISharedPipe<IMessage>> pipeFactory, bool shouldStopOnError)
        {
            _funWork             = funWork;
            _msgVisitor          = msgVisitorFactory.Create(this);
            OnEndOfStream       += OnEndOfStreamHandler;
            (_toReader, _writer) = pipeFactory().GetReaderWriterCouple();
            // Dispose handler
            _unsubscriber        = new Unsubscriber();
            // unsubscribe the event on dispose
            _unsubscriber.RegisterSubscription(() => OnEndOfStream -= OnEndOfStreamHandler);
        }

        public void Start() => _fromReader.Reader.Read();

        private void OnEndOfStreamHandler(object sender, EndOfStreamEventArg arg) => Push(arg.EOS);
        #endregion

        #region IPipeable<IMessage> implementation
        public void PipeFrom(IReadable<IMessage> reader)
        {
            _fromReader = reader;
            if (_fromReader.Reader.TryRegisterReaderHandler((msg) => msg.Accept(_msgVisitor), out int id))
            {
                void UnsubscribeFromReader()
                {
                    _fromReader.Reader.TryUnregisterReaderHandler(id);
                    _fromReader.Dispose();
                }
                // unsubscribe the message handler, then dispose the from reader.
                _unsubscriber.RegisterSubscription(UnsubscribeFromReader);
            }
        }
        #endregion

        #region IReadable<IMessage> implementation
        IReader<IMessage> IReadable<IMessage>.Reader => _toReader;

        public void Dispose() => _unsubscriber.Unsubscribe(); 
        #endregion
    }
}
