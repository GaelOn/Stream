using System;
using Stream.Contract;

namespace Stream.Streams
{
    internal sealed class IterEndOfStream<TIn> : IEndOfStream
    {
        private Action<TIn> _fn;
        private IStartable _parent;
        private bool _haveBeenStarted = false;

        private IterEndOfStream(Action<TIn> fn, IStartable parent)
        {
            _fn = fn;
            _parent = parent;
        }

        public void Start()
        {
            _parent.Start();
            _haveBeenStarted = true;
        }

        public void Return()
        {
            if (!_haveBeenStarted)
            {
                Start();
            }
            return;
        }

        private void Flow(TIn input) => _fn(input);

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (IEndOfStream EndOfStream, Action<TIn> Pusher)
            GetIterEndOfStream(Action<TIn> fn, IStartable parent)
        {
            var tstream = new IterEndOfStream<TIn>(fn, parent);
            return (tstream, tstream.Flow);
        }
    }
}
