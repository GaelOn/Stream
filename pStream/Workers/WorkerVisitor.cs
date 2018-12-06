using pStream.Workers;
using System;

namespace pStream.Messages
{
    internal class WorkerVisitor<TIn> : IMessageVisitor
    {
        private readonly IWorker<TIn> worker;

        public WorkerVisitor(IWorker<TIn> worker) => this.worker = worker;

        public void VisitErrorMessage<TExcep>(IErrorMessage<TExcep> msg) where TExcep : Exception => worker.Push(msg);

        public void VisitInputMessage(IMessage msg)
        {
            IMessage mess;
            try
            {
                mess = worker.DoWork((msg as InputMessage<TIn>).Value);
            }
            catch (Exception e)
            {
                mess = new ErrorMessage<Exception>(e);
            }
            worker.Push(mess);
        }

        public void VisitEOS(EndOfStreamMessage msg) => worker.RaiseOnEndOfStream();
    }
}
