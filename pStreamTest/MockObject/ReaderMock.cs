using System;
using pStream.Pipeline;

namespace StreamTest.MockObject
{
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

}