using System;
using System.Threading;

namespace pStream.WaitStrategy
{
    class AutoResetWaitOnceStrategy : IWaitOnceStrategy
    {
        private readonly AutoResetEvent _mre;

        public AutoResetWaitOnceStrategy(bool initialState = false)
        {
            _mre = new AutoResetEvent(initialState);
        }

        public void Signal()
        {
            _mre.Set();
        }

        public void WaitOnce(Action runner)
        {
            _mre.WaitOne();
            runner();
        }
    }
}
