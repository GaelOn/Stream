namespace Stream.Contract
{
    public interface IConsumerSourceStream<T>
    {
        void Consume(T elem);
        void Consume(T[] elems);
    }
}