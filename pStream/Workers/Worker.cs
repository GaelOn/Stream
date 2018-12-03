using System;
using System.Collections.Generic;

namespace pStream.Workers
{
    internal sealed class Worker<TIn, TOut> : IObserver<TIn>, IObservable<TOut>, IDisposable
    {
        private List<IObserver<TOut>> _observers;
        private readonly List<IDisposable> iDisposables;

        public Worker()
        {
            _observers = new List<IObserver<TOut>>(8);
            iDisposables = new List<IDisposable>(8);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TIn value)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<TOut> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public void Dispose()
        {
            foreach(var elem in iDisposables)
            {
                elem.Dispose();
            }
        }

        /// <summary>
        /// based on https://msdn.microsoft.com/fr-fr/library/dd782981(v=vs.110).aspx
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private WeakReference<List<IObserver<TOut>>> _observers;
            private WeakReference<IObserver<TOut>> _observer;
            private IObserver<TOut> observerPlaceHolder;
            private List<IObserver<TOut>> observersPlaceHolder;

            private List<IObserver<TOut>> Observers
            {
                get
                {
                    var getit = _observers.TryGetTarget(out observersPlaceHolder);
                    if (getit) { observersPlaceHolder = null; }
                    return observersPlaceHolder;
                }
            }

            private IObserver<TOut> Observer
            {
                get
                {
                    var getit = _observer.TryGetTarget(out observerPlaceHolder);
                    if (getit) { observerPlaceHolder = null; }
                    return observerPlaceHolder;
                }
            }

            public Unsubscriber(List<IObserver<TOut>> observers, IObserver<TOut> observer)
            {
                _observers = new WeakReference<List<IObserver<TOut>>>(observers);
                _observer  = new WeakReference<IObserver<TOut>>(observer);
            }

            public void Dispose()
            {
                if (Observer != null && Observers.Contains(Observer))
                {
                    Observers.Remove(Observer);
                    _observer = null;
                }
                _observers = null;
            }
        }
    }
}
