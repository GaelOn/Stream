namespace pStream.Messages
{
    internal class EndOfStreamMessage : IMessage
    {
        private static EndOfStreamMessage _instance;

        static EndOfStreamMessage() => _instance = new EndOfStreamMessage();

        public static EndOfStreamMessage Value => _instance;

        private EndOfStreamMessage() { }

        public void Accept(IMessageVisitor visitor) => visitor.VisitEOS(this);
    }
}
