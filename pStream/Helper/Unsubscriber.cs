using System;
using System.Collections.Generic;

namespace pStream.Helper
{
    class Unsubscriber : IUnsubscriber
    {
        private readonly List<Action> _unsubscribeActions;

        public Unsubscriber() => _unsubscribeActions = new List<Action>(10);

        public void RegisterSubscription(Action unsubscribe) => _unsubscribeActions.Add(unsubscribe);

        public void Unsubscribe() => _unsubscribeActions.ForEach((unsubscribe) => unsubscribe());
    }
}
