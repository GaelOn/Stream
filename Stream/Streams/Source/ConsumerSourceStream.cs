using System;
using System.Collections.Generic;
using Stream.Extension;

namespace Stream.Streams.Source
{
    public class ConsumerSourceStream<T> : Stream<T, T[]>, IConsumerSourceStream<T>
    {
        protected List<T> _internal;
        protected int _readIt;
        private Action<T> _pushInternal;

        private ConsumerSourceStream(int bufferSize)
        {
            _internal = new List<T>(bufferSize);
            _pushInternal = (x => _internal.Add(x));
            _streamer = _pushInternal;
        }

        protected override void Push(T input) => _streamer(input);

        public override void Start()
        {
            if (_streamer == _pushInternal)
            {
                return;
            }
            foreach (var elem in _internal)
            {
                Push(elem);
            }
        }

        public override T[] Result()
        {
            return _internal.ToArray();
        }

        public void Consume(T elem) => Push(elem);

        public void Consume(T[] elems) => elems.AsStream().Iter(x => _streamer(x));

        private class ConsumerStreamProxy<TSource> : IConsumerSourceStream<TSource>
        {
            private readonly IConsumerSourceStream<TSource> _css;

            public ConsumerStreamProxy(IConsumerSourceStream<TSource> css) => _css = css;

            public void Consume(TSource elem) => _css.Consume(elem);

            public void Consume(TSource[] elems) => _css.Consume(elems);
        }

        private IConsumerSourceStream<T> GetConsumerProxy() => new ConsumerStreamProxy<T>(this);

        // Tuple<IConsumerSourceStream<T>, Stream<T, T[]>>
        public static (IConsumerSourceStream<T> consumer, Stream<T, T[]> stream) GetConsumerSourceStream(int bufferSize = 32)
        {
            var tstream = new ConsumerSourceStream<T>(bufferSize);
            return (new ConsumerStreamProxy<T>(tstream), tstream);
        }
    }
}
