using pStream.Pipeline;
using pStream.Workers;

namespace StreamTest.MockObject
{
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

}