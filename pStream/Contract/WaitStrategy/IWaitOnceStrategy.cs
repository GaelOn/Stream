using System;

namespace pStream.WaitStrategy
{
    interface IWaitOnceStrategy : IControlableWaitStrategy
    {
        void WaitOnce(Action runner);
    }
}