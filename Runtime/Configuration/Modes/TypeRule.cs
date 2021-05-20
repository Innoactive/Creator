using System;

namespace VPG.Core.Configuration.Modes
{
    /// <summary>
    /// An <see cref="IRule{T}"/> for types.
    /// </summary>
    /// <typeparam name="TValueBase">A type from which all input types have to be inherited from.</typeparam>
    public abstract class TypeRule<TValueBase> : IRule<Type>
    {
        /// <summary>
        /// Generic version of <see cref="IsQualifiedBy"/>.
        /// </summary>
        public bool IsQualifiedBy<T>() where T : TValueBase
        {
            return IsQualifiedBy(typeof(T));
        }

        /// <inheritdoc />
        public bool IsQualifiedBy(Type type)
        {
            if (typeof(TValueBase).IsAssignableFrom(type) == false)
            {
                return false;
            }

            return IsQualifiedByPredicate(type);
        }

        /// <summary>
        /// Actual check of a given type. It is guaranteed that <paramref name="type"/> inherits the <typeparam name="TValueBase" />
        /// </summary>
        protected abstract bool IsQualifiedByPredicate(Type type);
    }
}
