namespace pStream.Pipeline
{
    interface IWriter<TOut>
    {
        void Push(TOut output);
        void Stop();
    }
}
