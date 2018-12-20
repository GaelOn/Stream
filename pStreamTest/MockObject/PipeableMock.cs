using System;
using System.Collections.Generic;
using pStream.Helper;
using pStream.Pipeline;
using pStream.Workers;

namespace StreamTest.MockObject
{
    class PipeableMock<TIn> : IPipeable<TIn>, IDisposable
    {
        #region Private Variables
        private readonly IUnsubscriber _unsubscriber = new Unsubscriber();
        private          IReadable<TIn> _reader;
        #endregion

        public PipeableMock() => Results = new List<TIn>(1);

        public IList<TIn> Results { get; }

        #region Implementation of IPipeable<TIn>
        public void PipeFrom(IReadable<TIn> reader)
        {
            _reader = reader;
            if (_reader.Reader.TryRegisterReaderHandler((msg) => Results.Add(msg), out int id))
            {
                _unsubscriber.RegisterSubscription(() => _reader.Reader.TryUnregisterReaderHandler(id));
            }            
        }
        #endregion

        public void Read() => _reader.Reader.Read();

        public void Dispose() => _unsubscriber.Unsubscribe();
    }

    class ReadableMock<TIn> : IReadable<TIn>
    {
        private readonly ISharedPipe<TIn> _var;

        public ReadableMock()
        {
            _var = new SharedPipeMock<TIn>();
            (Reader, Writer) = _var.GetReaderWriterCouple();
        }

        public IReader<TIn> Reader { get; }

        public IWriter<TIn> Writer { get; }

        public void Dispose() { }
    }

    class WriterMock<TIn> : IWriter<TIn>
    {
        Action<TIn> _writer;
        bool        _shouldRead;

        public WriterMock(Action<TIn> writer) => _writer = writer;

        public void Push(TIn output)
        {
            _writer(output);
            _shouldRead = true;
        }

        public void Stop() => _writer = null;

        public bool TriggerRead() => _shouldRead;
    }

    class ReaderMock<TIn> : IReader<TIn>
    {
        private Action<TIn> _observers;
        private readonly Func<TIn>   _reader;
        private readonly Func<bool>  _shouldRead;

        public ReaderMock(Func<TIn> reader, Func<bool> shouldRead)
        {
            _reader     = reader;
            _shouldRead = shouldRead;
        }

        public void Read()
        {
            if (_shouldRead())
            { 
                Reader();
            }
        }

        public bool TryRegisterReaderHandler(Action<TIn> onNewElement, out int id)
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

        private bool Reader()
        {
            if (_observers == null)
            {
                return true;
            }
            _observers(_reader());
            return false;
        }
    }

    class SharedPipeMock<TIn> : ISharedPipe<TIn>
    {
        private TIn _pseudoQueue;

        public IReader<TIn> GetReader() => new ReaderMock<TIn>(() => _pseudoQueue, () => true);

        public (IReader<TIn>, IWriter<TIn>) GetReaderWriterCouple() => (GetReader(), GetWriter());

        public IWriter<TIn> GetWriter() => new WriterMock<TIn>((value) => _pseudoQueue = value);
    }

}