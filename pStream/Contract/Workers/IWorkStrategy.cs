using System;
using pStream.Messages;

namespace pStream.Contract.Workers
{
    internal interface IWorkStrategy : IDisposable
    {
        void Push(IMessage input);
    }
}