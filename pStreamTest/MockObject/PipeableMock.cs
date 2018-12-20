using System;
using System.Collections.Generic;
using pStream.Helper;
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
}