using System;

namespace Innoactive.Creator.Core
{
    public class ChildDeactivatedEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity Child { get; private set; }

        public ChildDeactivatedEventArgs(TEntity child)
        {
            Child = child;
        }
    }
}
