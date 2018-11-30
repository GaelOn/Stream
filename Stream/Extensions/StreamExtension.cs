using Stream.Streams.Source;

namespace Stream.Extension
{
    public static class StreamExtension
    {
        public static SourceStream<T> AsStream<T>(this T[] tab)
        {
            return new SourceStream<T>(tab);
        }
    }
}
