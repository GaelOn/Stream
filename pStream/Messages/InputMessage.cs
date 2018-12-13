namespace pStream.Messages
{
    internal class InputMessage<TIn> : IMessageValue<TIn>
    {
        private readonly TIn _value;
        public TIn Value => _value;

        public InputMessage(TIn value) => _value = value;

        public void Accept(IMessageVisitor visitor) => visitor.VisitInputMessage(this);
    }
}
