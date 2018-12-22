namespace Stream
{
    internal abstract class AccumulableStream<TIn, TOut> : Stream<TOut, TOut>
    {
        protected readonly IStartable _parent;
        protected TOut _accumulator;

        protected bool IsInit { get; private set; }

        protected AccumulableStream(IStartable parent)
        {
            IsInit = false;
            _parent = parent;
        }

        protected abstract void Flow(TIn input);

        protected TOut GetResult()
        {
            IsInit = true;
            _parent.Start();
            return _accumulator;
        }
    }
}
