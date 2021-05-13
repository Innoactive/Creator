using System;

namespace VPG.Creator.Core
{
    [Obsolete("This event is not used anymore.")]
    public class ChildActivatedEventArgs<TEntity> : EventArgs where TEntity : IEntity
    {
        public TEntity Child { get; private set; }

        public ChildActivatedEventArgs(TEntity child)
        {
            Child = child;
        }
    }
}
