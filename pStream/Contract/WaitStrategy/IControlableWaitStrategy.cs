namespace pStream.WaitStrategy
{
    internal interface IControlableWaitStrategy : IWaitStrategy
    {
        void Signal();
    }
}
