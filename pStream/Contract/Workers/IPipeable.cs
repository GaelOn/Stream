namespace pStream.Workers
{

    internal interface IPipeable<TIn>
    {
        void PipeFrom(IReadable<TIn> reader);
    }
}
