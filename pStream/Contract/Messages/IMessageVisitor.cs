using System;

namespace pStream.Messages
{
    interface IMessageVisitor
    {
        void VisitErrorMessage<TExcep>(IErrorMessage<TExcep> msg) where TExcep : Exception;
        void VisitInputMessage(IMessage msg);
        void VisitEOS(EndOfStreamMessage msg);
    }
}
