using pStream.Workers;

namespace pStream.Messages
{
    interface IMessageVisitorFactory
    {
        IMessageVisitor Create<TIn>(IWorker<TIn> worker);
    }
}
