namespace Stream.Streams.Source {
    public sealed class SourceStream<T> : Stream<T, T[]> {
        private T[] _internal;

        public SourceStream (T[] array) => _internal = array;

        protected override void Push (T input) => _streamer (input);

        public override void Start () {
            for (var it = 0; it < _internal.GetLength (0); it++) {
                Push (_internal[it]);
            }
        }

        public override T[] Result () {
            return _internal;
        }
    }
}