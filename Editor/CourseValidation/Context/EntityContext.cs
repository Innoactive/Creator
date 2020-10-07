using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IData"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="IData"/> which is the context scope.</typeparam>
    public abstract class EntityContext<T> : IContext where T : IData
    {
        /// <inheritdoc/>
        public IContext Parent { get; }

        /// <summary>
        /// The course entity this context is about.
        /// </summary>
        public T Entity { get; }

        /// <inheritdoc />
        public abstract bool IsSelectable { get; }

        public EntityContext(T entity, IContext parent)
        {
            Entity = entity;
            Parent = parent;
        }

        /// <inheritdoc />
        public abstract void Select();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityContext<T>) obj);
        }

        protected bool Equals(EntityContext<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Entity, other.Entity);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Entity);
        }
    }
}
