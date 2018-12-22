using System;

namespace Stream.Streams.Folder
{
    internal sealed class AverageStream<TIn> : SumDoubleStream<TIn>
    {
        private int _count;

        public AverageStream(Func<TIn, double> fn, IStartable parent) : base(fn, parent) { }

        protected override void Flow(TIn input)
        {
            _accumulator = _fn(input, _accumulator);
            _count++;
        }

        public override double Result()
        {
            if (!IsInit)
            {
                GetResult();
            }
            return _accumulator / _count;
        }

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<double, double> Stream, Action<TIn> Pusher) GetAverageStreamm(Func<TIn, double> fn, IStartable parent)
        {
            var tstream = new AverageStream<TIn>(fn, parent);
            return (tstream as Stream<double, double>, tstream.Flow);
        }
    }
}
