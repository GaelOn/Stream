using System;
namespace pStream.WaitStrategy
{
    internal interface IWaitStrategy
    {
        void Run(Func<bool> runner);
    }
}
