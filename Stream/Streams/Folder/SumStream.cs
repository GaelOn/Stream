using System;
using Stream.Contract;

namespace Stream.Streams.Folder
{
    internal class SumDoubleStream<TIn> : FolderStream<TIn, double>
    {
        public SumDoubleStream(Func<TIn, double> fn, IStartable parent)
            : base((o, acc) => fn(o) + acc, 0d, parent) { }

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<double, double> Stream, Action<TIn> Pusher) GetSumDoubleStream(Func<TIn, double> fn, IStartable parent)
        {
            var tstream = new SumDoubleStream<TIn>(fn, parent);
            return (tstream as Stream<double, double>, tstream.Flow);
        }
    }

    internal class SumIntStream<TIn> : FolderStream<TIn, int>
    {
        public SumIntStream(Func<TIn, int> fn, IStartable parent)
            : base((o, acc) => fn(o) + acc, 0, parent) { }

        // Tuple<TargetStream<T, T2>, Action<T>>
        public static (Stream<int, int> Stream, Action<TIn> Pusher) GetSumIntStream(Func<TIn, int> fn, IStartable parent)
        {
            var tstream = new SumIntStream<TIn>(fn, parent);
            return (tstream as Stream<int, int>, tstream.Flow);
        }
    }
}
