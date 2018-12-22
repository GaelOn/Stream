using System;

namespace Stream.Streams
{
    internal sealed class ManyTargetStream<TIn, TOut> : FlowStream<TIn, TOut>
    {
        private Func<TIn, Stream<TOut, TOut[]>> _fn;

        private ManyTargetStream(Func<TIn, Stream<TOut, TOut[]>> fn, IStartable parent)
            : base(parent) => _fn = fn;

        protected override void Flow(TIn input)
        {
            var str = _fn(input);
            var eos = str.Iter((x => _streamer(x)));
            eos.Start();
        }

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<TOut, TOut[]> Stream, Action<TIn> Pusher)
            GetManyTargetStream(Func<TIn, Stream<TOut, TOut[]>> fn, IStartable parent)
        {
            var tstream = new ManyTargetStream<TIn, TOut>(fn, parent);
            return (tstream as Stream<TOut, TOut[]>, tstream.Flow);
        }
    }
}
