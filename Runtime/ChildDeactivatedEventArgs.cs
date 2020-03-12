using System;

namespace Innoactive.Hub.Training
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
