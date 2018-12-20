using pStream.Pipeline;

namespace StreamTest.MockObject
{
    class SharedPipeMock<TIn> : ISharedPipe<TIn>
    {
        private TIn _pseudoQueue;

        public IReader<TIn> GetReader() => new ReaderMock<TIn>(() => _pseudoQueue, () => true);

        public (IReader<TIn>, IWriter<TIn>) GetReaderWriterCouple() => (GetReader(), GetWriter());

        public IWriter<TIn> GetWriter() => new WriterMock<TIn>((value) => _pseudoQueue = value);
    }

}