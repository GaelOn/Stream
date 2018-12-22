using pStream.Pipeline;
using pStream.Workers;

namespace StreamTest.MockObject
{
    class ReadableMock<TIn> : IReadable<TIn>
    {
        private readonly ISharedPipe<TIn> _var;

        /// <summary>
        /// Create a default internal non thread safe variable
        /// </summary>
        public ReadableMock()
        {
            _var = new SharedPipeMock<TIn>();
            (Reader, Writer) = _var.GetReaderWriterCouple();
        }

        /// <summary>
        /// Use SharedVar pass as argument as internal variable. Should be thread safe.
        /// </summary>
        /// <param name="var">object of type <paramref SharedVar> use to produce Reader and Writer. </paramref></param>
        public ReadableMock(SharedVar<TIn> var)
        {
            _var = var;
            (Reader, Writer) = _var.GetReaderWriterCouple();
        }
        
        public IReader<TIn> Reader { get; }

        public IWriter<TIn> Writer { get; }

        public void Dispose() { }
    }

}