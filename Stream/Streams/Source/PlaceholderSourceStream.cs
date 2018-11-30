using System;
namespace Stream.Streams.Source
{
    public class PlaceholderSourceStream<T> : Stream<T, T>
    {
        public PlaceholderSourceStream()
        {
        }

        public override T Result()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        protected override void Push(T input)
        {
            throw new NotImplementedException();
        }
    }
}
