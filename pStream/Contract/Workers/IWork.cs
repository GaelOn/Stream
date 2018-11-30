namespace pStream.Contract.Workers
{
    interface IWork
    {
        bool Read();
        void Start();
    }
}