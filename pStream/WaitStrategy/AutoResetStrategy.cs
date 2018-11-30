using System;
using System.Threading;

namespace pStream.WaitStrategy
{
    public class AutoResetStrategy : IControlableWaitStrategy
    {
        private bool _shouldStop = false;
        private readonly AutoResetEvent _mre = new AutoResetEvent(false);

        public void Run(Func<bool> runner)
        {
            while (!_shouldStop)
            {
                _shouldStop = runner();
                _mre.WaitOne();
            }
        }

        public void Signal()
        {
            _mre.Set();
        }
    }
}
