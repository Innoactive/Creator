using System;

namespace Innoactive.Creator.Core
{
    public class CurrentChildChangedEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity NewChild { get; private set; }

        public CurrentChildChangedEventArgs(TEntity newChild)
        {
            NewChild = newChild;
        }
    }
}
