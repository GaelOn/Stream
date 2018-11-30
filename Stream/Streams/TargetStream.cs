using System;
using Stream.Contract;

namespace Stream.Streams
{
    internal sealed class TargetStream<TIn, TOut> : FlowStream<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _fn;

        private TargetStream(Func<TIn, TOut> fn, IStartable parent)
            : base(parent) => _fn = fn;

        protected override void Flow(TIn input) => _streamer(_fn(input));

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<TOut, TOut[]> Stream, Action<TIn> Pusher) GetTargetStream(Func<TIn, TOut> fn, IStartable parent)
        {
            var tstream = new TargetStream<TIn, TOut>(fn, parent) ;
            return (tstream as Stream<TOut, TOut[]>, tstream.Flow);
        }
    }
}
