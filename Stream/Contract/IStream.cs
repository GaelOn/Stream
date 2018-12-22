using System;

namespace Stream
{
    public interface IStream<TOut, TResult>
    {
        Stream<TOutNext, TOutNext[]> Map<TOutNext>(Func<TOut, TOutNext> mapper);
		Stream<TIn, TIn[]> FlatMap<TIn>(Func<TOut, Stream<TIn, TIn[]>> fn);
        Stream<TOut, TOut[]> Filter(Func<TOut, bool> filter);
        Stream<TIn, TIn> Fold<TIn>(Func<TOut, TIn, TIn> fn, TIn initAccValue);
		IEndOfStream Iter(Action<TOut> fn);
        Stream<TIn, TIn> Collect<TIn, TAcc>(Func<TIn> initializer, Func<TOut, TAcc> accumulator, Action<TAcc, TIn> appender);
        Stream<int, int> Sum(Func<TOut, int> fn);
        Stream<double, double> Sum(Func<TOut, double> fn);
        Stream<double, double> Average(Func<TOut, double> fn);
        Stream<double, double> Max(Func<TOut, double> fn);
        Stream<double, double> Min(Func<TOut, double> fn);
        TResult Result();
    }
}