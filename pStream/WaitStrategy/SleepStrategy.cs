using System;
using System.Threading;

namespace pStream.WaitStrategy
{
    public class SleepStrategy : IWaitStrategy
    {
        bool _shouldStop = false;

        public void Run(Func<bool> runner)
        {
            while (!_shouldStop)
            {
                _shouldStop = runner();
                Thread.Sleep(0);
            }
        }
    }
}
