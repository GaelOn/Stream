using System;
using System.Threading;
using pStream.WaitStrategy;

namespace pStream.Pipeline
{
    class SharedVar<TIn> : ISharedPipe<TIn>
    {
        private readonly Synchronizer      _syncRoot        = new Synchronizer(false);
        private readonly IWaitStrategy     _readerAwaiter   = new AutoResetStrategy();
        private readonly IWaitOnceStrategy _writerAwaiter   = new AutoResetWaitOnceStrategy(true);
        private          TIn               _pseudoQueue;

        public IReader<TIn> GetReader() => new SharedReader<TIn>(_readerAwaiter, _writerAwaiter, _syncRoot, () =>_pseudoQueue);

        public IWriter<TIn> GetWriter() => new SharedWriter<TIn>(_readerAwaiter,  _writerAwaiter, _syncRoot, (value) => _pseudoQueue = value);

        public (IReader<TIn>, IWriter<TIn>) GetReaderWriterCouple() => (GetReader(), GetWriter());

        //private void Read(Func<bool> reader) => _readerAwaiter.Run(reader);

        private class Synchronizer
        {
            private bool _value;
            public Synchronizer(bool initValue) => _value = initValue;

            public bool Read() => Volatile.Read(ref _value);

            public void Write(bool newValue) => Volatile.Write(ref _value, newValue);
        }

        private class SharedReader<TRead> : IReader<TRead>
        {
            #region Private variable

            private          Action<TRead>     _observers;
            private readonly Func<TRead>       _reader;
            private readonly IWaitStrategy     _waitHandleReader;
            private readonly IWaitOnceStrategy _waitHandleWriter;
            private readonly Synchronizer      _syncRoot;

            #endregion

            public SharedReader(IWaitStrategy waitHandleReader, IWaitOnceStrategy waitHandleWriter, Synchronizer syncRoot, Func<TRead> reader)
            {
                _waitHandleReader = waitHandleReader;
                _waitHandleWriter = waitHandleWriter;
                _reader           = reader;
                _syncRoot         = syncRoot;
            }

            #region Implementation of IReader<TIn>

            public void Read() => _waitHandleReader.Run(Reader);

            public bool TryRegisterReaderHandler(Action<TRead> onNewElement, out int id)
            {
                if (_observers != null)
                {
                    id = -1;
                    return false;
                }
                _observers = onNewElement;
                id = 0;
                return true;
            }

            public bool TryUnregisterReaderHandler(int id)
            {
                if (id != 0)
                {
                    return false;
                }
                _observers = null;
                return true;
            } 

            #endregion

            private bool Reader()
            {
                if (_observers == null)
                {
                    return true;
                }
                _observers(_reader());
                _waitHandleWriter.Signal();
                return _syncRoot.Read() || false;
            }
        }

        private class SharedWriter<TWrite> : IWriter<TWrite>
        {
            #region Private variable

            private readonly Action<TWrite>    _write;
            private readonly IWaitStrategy     _waitHandleReader;
            private readonly IWaitOnceStrategy _waitHandleWriter;
            private readonly Synchronizer      _syncRoot;
            //private          TWrite            _buffer;

            #endregion

            public SharedWriter(IWaitStrategy waitHandleReader, IWaitOnceStrategy waitHandleWriter, Synchronizer syncRoot, Action<TWrite> write)
            {
                _waitHandleReader = waitHandleReader;
                _waitHandleWriter = waitHandleWriter;
                _syncRoot         = syncRoot;
                _write            = write;
                
            }

            #region IWriter<TWrite>

            public void Push(TWrite input) => _waitHandleWriter.WaitOnce(() => Writer(input));

            public void Stop()
            {
                var _true = true;
                _syncRoot.Write(_true);
                _waitHandleReader.Signal();
            }
            #endregion

            private void Writer(TWrite input)
            {
                _write(input);
                _waitHandleReader.Signal();
            }
        }
    }
}
