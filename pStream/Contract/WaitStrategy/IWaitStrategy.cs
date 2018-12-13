using System;
namespace pStream.WaitStrategy
{
    internal interface IWaitStrategy : IControlableWaitStrategy
    {
        void Run(Func<bool> runner);
    }
}
