using pStream.Messages;

namespace pStream.Workers
{
    internal interface IWorker<TIn>
    {
        event OnEndOfStreamHandler OnEndOfStream;        
        void Start();                
        void RaiseOnEndOfStream();
        void Push(IMessage msg);
        IMessage DoWork(TIn entry);
    }
}