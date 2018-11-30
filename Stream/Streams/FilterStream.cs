using System;
using Stream.Contract;

namespace Stream.Streams
{
    internal sealed class FilterStream<T> : FlowStream<T, T>
    {
        private readonly Func<T, bool> _filter;

        private FilterStream(Func<T, bool> filter, IStartable parent) : base(parent)
        {
            _filter = filter;
        }

        protected override void Flow(T input)
        {
            if (_filter(input))
            {
                _streamer(input);
            }
        }

        public static (Stream<T, T[]> Stream, Action<T> Pusher) GetFilterStream(Func<T, bool> filter, IStartable parent)
        {
            var fstream = new FilterStream<T>(filter, parent);
            return (fstream as Stream<T, T[]>, fstream.Flow);
        }
    }
}
