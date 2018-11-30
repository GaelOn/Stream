using System;
using System.Collections.Generic;
using Stream.Contract;

namespace Stream.Streams
{
    internal class ResultStream<T> : FlowStream<T, T>
    {
        private List<T> _internal = new List<T>();

        public ResultStream(IStartable parent) : base(parent) { }

        protected override void Flow(T input) => _internal.Add(input);
		
        public override T[] Result() => _internal.ToArray();

        public static (IStartable Stream, Action<T> Pusher) 
            GetResultStream(IStartable parent)
        {
            var fstream = new ResultStream<T>(parent);
            return (fstream as Stream<T, T[]>, fstream.Flow);
        }
    }
}
