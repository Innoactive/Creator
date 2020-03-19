using System;

namespace Innoactive.Creator.Core
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
