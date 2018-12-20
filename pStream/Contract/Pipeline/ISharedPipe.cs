namespace pStream.Pipeline
{
    interface ISharedPipe<TElem>
    {
        IWriter<TElem> GetWriter();
        IReader<TElem> GetReader();
        (IReader<TElem>, IWriter<TElem>) GetReaderWriterCouple();
    }
}
