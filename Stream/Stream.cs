using System;
using Stream.Contract;
using Stream.Streams;
using Stream.Streams.Folder;

namespace Stream 
{
    public abstract class Stream<TOut, TResult> : IStartable, IStream<TOut, TResult> 
    {
        protected Action<TOut> _streamer;
        internal ResultStream<TOut> _result;

        public Stream<TIn, TIn[]> Map<TIn> (Func<TOut, TIn> mapper) 
        {
            var (Stream, Pusher) = TargetStream<TOut, TIn>.GetTargetStream (mapper, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<TOut, TOut[]> Filter (Func<TOut, bool> filter) 
        {
            var (Stream, Pusher) = FilterStream<TOut>.GetFilterStream (filter, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<TIn, TIn> Fold<TIn> (Func<TOut, TIn, TIn> fn, TIn initAccValue) 
        {
            var (Stream, Pusher) = FolderStream<TOut, TIn>.GetAccumulableStream (fn, initAccValue, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<int, int> Sum (Func<TOut, int> fn) 
        {
            var (Stream, Pusher) = SumIntStream<TOut>.GetSumIntStream (fn, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<double, double> Sum (Func<TOut, double> fn) 
        {
            var (Stream, Pusher) = SumDoubleStream<TOut>.GetSumDoubleStream (fn, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<double, double> Average (Func<TOut, double> fn) 
        {
            var (Stream, Pusher) = AverageStream<TOut>.GetAverageStreamm (fn, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<double, double> Max (Func<TOut, double> fn) 
        {
            Func<TOut, double, double> folder = ((x, acc) => { var nx = fn (x); return nx > acc ? nx : acc; });
            var (Stream, Pusher) = FolderStream<TOut, double>.GetAccumulableStream (folder, double.MinValue, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<double, double> Min (Func<TOut, double> fn) 
        {
            Func<TOut, double, double> folder = ((x, acc) => { var nx = fn (x); return nx < acc ? nx : acc; });
            var (Stream, Pusher) = FolderStream<TOut, double>.GetAccumulableStream (folder, double.MaxValue, this);
            _streamer = Pusher;
            return Stream;
        }

        public Stream<TIn, TIn[]> FlatMap<TIn>(Func<TOut, Stream<TIn, TIn[]>> fn) 
        {
            var (Stream, Pusher) = ManyTargetStream<TOut, TIn>.GetManyTargetStream (fn, this);
            _streamer = Pusher;
            return Stream;
        }

        public IEndOfStream Iter(Action<TOut> fn)
        {
            var (EndOfStream, Pusher) = IterEndOfStream<TOut>.GetIterEndOfStream(fn, this);
            _streamer = Pusher;
            return EndOfStream;
        }

        public Stream<TIn, TIn> Collect<TIn, TAcc>(Func<TIn> initializer, Func<TOut, TAcc> accumulator, Action<TAcc, TIn> appender)
        {
            var (Stream, Pusher) = CollectorStream<TOut, TAcc, TIn>.GetCollectorStream(initializer, accumulator, appender, this);
            _streamer = Pusher;
            return Stream;
        }

        public abstract void Start ();

        protected abstract void Push (TOut input);

        public abstract TResult Result ();

        protected void InitResult () 
        {
            var (Stream, Pusher) = ResultStream<TOut>.GetResultStream (this);
            _streamer = Pusher;
            _result = Stream as ResultStream<TOut>;
        }
    }
}