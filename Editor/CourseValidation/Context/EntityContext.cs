using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IEntity"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="IEntity"/> which is the context scope.</typeparam>
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
            if (obj == null)
            {
                return false;
            }

            if (obj is EntityContext<T> context)
            {
                return Entity.Equals(context.Entity);
            }
            return false;
        }
    }
}
