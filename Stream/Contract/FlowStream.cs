namespace Stream
{
    internal abstract class FlowStream<TIn, TOut> : Stream<TOut, TOut[]>
    {
        protected readonly IStartable _parent;

        protected FlowStream(IStartable parent)
        {
            _parent = parent;
        }

        protected abstract void Flow(TIn input);

		public override void Start() 
        {
            if (_streamer == null )
            {
                InitResult();
            }
            _parent.Start();
        }

        public override TOut[] Result()
        {
            if (_result == null)
            {
                InitResult();
                Start();
            }
            return _result.Result();
        }

        protected override void Push(TOut input) => _streamer(input);
    }
}
