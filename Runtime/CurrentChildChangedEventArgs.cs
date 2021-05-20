using System;

namespace VPG.Core
{
    [Obsolete("This event is not used anymore.")]
    public class CurrentChildChangedEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity NewChild { get; private set; }

        public CurrentChildChangedEventArgs(TEntity newChild)
        {
            NewChild = newChild;
        }
    }
}
