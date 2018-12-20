using System;

namespace pStream.Helper
{
    /// <summary>
    /// To help into disposable scenario.
    /// </summary>
    interface IUnsubscriber
    {
        void Unsubscribe();
        void RegisterSubscription(Action unsubscribe);
    }
}
