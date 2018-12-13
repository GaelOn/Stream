using System;
using System.Threading;

namespace pStream.WaitStrategy
{
    public class AutoResetStrategy : IWaitStrategy
    {
        private bool _shouldStop = false;
        private readonly AutoResetEvent _mre;


        public AutoResetStrategy(bool initialState = false)
        {
            _mre = new AutoResetEvent(initialState);
        }

        public void Run(Func<bool> runner)
        {
            do
            {
                _shouldStop = runner();
                if (!_shouldStop)
                {
                    _mre.WaitOne();
                }
            }
            while (!_shouldStop);
        }

        public void Signal()
        {
            _mre.Set();
        }
    }
}
