using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IData"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="IData"/> which is the context scope.</typeparam>
    public abstract class EntityDataContext<T> : IContext where T : IData
    {
        /// <inheritdoc/>
        public IContext Parent { get; }

        /// <summary>
        /// The course entity's <see cref="IData"/> this context is about.
        /// </summary>
        public T EntityData { get; }

        protected EntityDataContext(T entityData, IContext parent)
        {
            EntityData = entityData;
            Parent = parent;
        }

        /// <inheritdoc/>
        public override bool Equals(object otherObject)
        {
            if (ReferenceEquals(null, otherObject)) return false;
            if (ReferenceEquals(this, otherObject)) return true;
            if (otherObject.GetType() != this.GetType()) return false;
            return Equals((EntityDataContext<T>) otherObject);
        }

        protected bool Equals(EntityDataContext<T> other)
        {
            return EqualityComparer<T>.Default.Equals(EntityData, other.EntityData);
        }


        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(EntityData);
        }
    }
}
