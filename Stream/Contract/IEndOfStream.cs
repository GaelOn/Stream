namespace Stream
{
    public interface IEndOfStream : IStartable
    {
        void Return();
    }
}