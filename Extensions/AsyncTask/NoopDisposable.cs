using System;

namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Very simple IDisposable implementation that does nothing when Dispose is called.
    /// Used for Asynchronous Tasks where the delegate does return a null disposable.
    /// </summary>
    internal class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
            // nothing to do here
        }
    }
}