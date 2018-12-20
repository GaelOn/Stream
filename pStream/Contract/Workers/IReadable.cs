using pStream.Pipeline;
using System;

namespace pStream.Workers
{
    internal interface IReadable<TOut> : IDisposable
    {
        IReader<TOut> Reader { get; }
    }
}
