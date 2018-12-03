namespace Stream.Streams.Folder
{
    public enum StreamMessageKind
    {
        None,
        Value,
        Stop
    }

    public enum MutableState
    {
        Unsetted = 0,
		Setted = 1
    }

    internal interface IMutable<T>
    {
        MutableState State { get; set; }
        void Set(T newValue);
        void Set();
        void Reset();
    }

    internal class StreamMessage<T> : IMutable<T>
    {
        //private bool _isInit;

        public StreamMessageKind MessageKind { get; private set; }
        
		MutableState IMutable<T>.State { get; set; }

		public T Value { get; private set; }

        public StreamMessage() => (this as IMutable<T>).Set();

        public StreamMessage(T value) => (this as IMutable<T>).Set(value);

        void IMutable<T>.Set(T newValue)
        {
            Value = newValue;
            MessageKind = StreamMessageKind.Value;
            (this as IMutable<T>).State = MutableState.Setted;
        }

        void IMutable<T>.Set() 
        {
			MessageKind = StreamMessageKind.Stop;
            (this as IMutable<T>).State = MutableState.Setted;
        }

        void IMutable<T>.Reset()
        {
            MessageKind = StreamMessageKind.None;
            Value = default(T);
            (this as IMutable<T>).State = MutableState.Unsetted;
        }
    }
}
