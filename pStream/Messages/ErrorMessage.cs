using System;

namespace pStream.Messages
{
    internal class ErrorMessage<TIn, TExcep> : IMessageValue<TIn>, IErrorMessage<TExcep> where TExcep : Exception
    {
        private readonly TExcep _err;

        TIn IMessageValue<TIn>.Value { get { throw _err; } }

        public TExcep Error => _err;

        public ErrorMessage(TExcep err) => _err = err;
    }
}
