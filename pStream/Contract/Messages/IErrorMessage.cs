using System;

namespace pStream.Messages
{
    internal interface IErrorMessage<out TExcep> : IMessage where TExcep : Exception
    {
        TExcep Error { get; }
    }
}
