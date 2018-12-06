namespace pStream.Messages
{
    internal interface IMessage
    {
        void Accept(IMessageVisitor visitor);
    }
}
