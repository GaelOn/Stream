using pStream.Messages;

namespace pStream.Workers
{
    internal class MessageVisitorFactory : IMessageVisitorFactory
    {
        public IMessageVisitor Create<TIn>(IWorker<TIn> worker) => new WorkerVisitor<TIn>(worker);
    }
}
