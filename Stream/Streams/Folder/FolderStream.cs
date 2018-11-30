using System;
using Stream.Contract;

namespace Stream.Streams.Folder
{
    internal class FolderStream<TIn, TOut> : AccumulableStream<TIn, TOut>
    {
        protected readonly Func<TIn, TOut, TOut> _fn;

        protected FolderStream(Func<TIn, TOut, TOut> fn, TOut initAccValue, IStartable parent) : base(parent)
        {
            _fn = fn;
            _accumulator = initAccValue;
        }

        protected override void Flow(TIn input) => _accumulator = _fn(input, _accumulator);

        public override void Start()
        {
            if (_streamer == null)
            {
                return;
            }
            _streamer(Result());
        }

        protected override void Push(TOut input) => _streamer(input);

        public override TOut Result()
        {
            if (!IsInit)
            {
                GetResult();
            }
            return _accumulator;
        }

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<TOut, TOut> Stream, Action<TIn> Pusher) GetAccumulableStream(Func<TIn, TOut, TOut> fn, TOut initAccValue, IStartable parent)
        {
            var tstream = new FolderStream<TIn, TOut>(fn, initAccValue, parent);
            return (tstream as Stream<TOut, TOut>, tstream.Flow);
        }
    }
}
