using System;
using pStream.Pipeline;

namespace StreamTest.MockObject
{
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

}