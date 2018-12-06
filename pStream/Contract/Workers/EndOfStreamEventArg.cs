using pStream.Messages;
using System;

namespace pStream.Workers
{
    internal delegate void OnEndOfStreamHandler(object sender, EndOfStreamEventArg e);

    internal class EndOfStreamEventArg : EventArgs
    {
        public EndOfStreamMessage EOS { get; }

        public EndOfStreamEventArg(EndOfStreamMessage eos) => EOS = eos;
    }
}