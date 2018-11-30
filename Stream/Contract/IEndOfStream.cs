namespace Stream.Contract
{
    public interface IEndOfStream : IStartable
    {
        void Return();
    }
}