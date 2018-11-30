namespace pStream.Messages
{
    internal interface IMessageValue<TIn> : IMessage
    {
        TIn Value { get; }
    }
}
