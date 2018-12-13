using System;

namespace pStream.Pipeline
{
    interface IReader<TIn>
    {
        bool TryRegisterReaderHandler(Action<TIn> onNewElement, out int id);
        bool TryUnregisterReaderHandler(int id);
        void Read();
    }
}
