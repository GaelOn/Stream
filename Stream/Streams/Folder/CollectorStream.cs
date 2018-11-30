using System;
using Stream.Contract;

namespace Stream.Streams.Folder
{
    internal sealed class CollectorStream<TIn, TAcc, TOut> : AccumulableStream<TIn, TOut>
    {
        private Func<TOut> _initializer;
        private Func<TIn, TAcc> _transformer;
        private Action<TAcc, TOut> _appender;
        private TOut _acc;
        
        public CollectorStream(Func<TOut> initializer, Func<TIn, TAcc> transformer, Action<TAcc, TOut> appender, IStartable parent)
            : base(parent)
        {
            _initializer = initializer;
            _transformer = transformer;
            _appender = appender;
            _acc = _initializer();
        }

        public override TOut Result()
        {
            if (!IsInit)
            {
                GetResult();
            }
            return _acc;
        }

        public override void Start()
        {
            {
                if (_streamer == null)
                {
                    return;
                }
                _streamer(Result());
            }
        }

        protected override void Flow(TIn input) => _appender(_transformer(input), _acc);

        protected override void Push(TOut input) => _streamer(input);

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<TOut, TOut> Stream, Action<TIn> Pusher) GetCollectorStream(Func<TOut> initializer, Func<TIn, TAcc> accumulator, Action<TAcc, TOut> appender, IStartable parent)
        {
            var tstream = new CollectorStream<TIn, TAcc, TOut>(initializer, accumulator, appender, parent);
            return (tstream as Stream<TOut, TOut>, tstream.Flow);
        }
    }
}
