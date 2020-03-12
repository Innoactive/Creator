using System;

namespace Innoactive.Hub.Training
{
    public class ChildActivatedEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity Child { get; private set; }

        public ChildActivatedEventArgs(TEntity child)
        {
            Child = child;
        }
    }
}
