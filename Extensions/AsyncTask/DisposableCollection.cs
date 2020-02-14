using System;
using System.Collections.ObjectModel;

namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Collection of objects implementing IDisposable interface.
    /// Calling dispose on this collection will dispose all contained items.
    /// </summary>
    public class DisposableCollection : Collection<IDisposable>, IDisposable
    {
        /// <summary>
        /// Disposes all of the items inside the collection.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable disposable in this)
            {
                disposable.Dispose();
            }

            Clear();
        }
    }
}