using System;

namespace pStream.Pipeline
{
    interface ISharedPipe<TElem>
    {
        IWriter<TElem> GetWriter();
        IReader<TElem> GetReader();
        Tuple<IReader<TElem>, IWriter<TElem>> GetReaderWriterCouple();
    }
}
