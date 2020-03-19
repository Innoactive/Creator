using System;

namespace Innoactive.Creator.Core.Configuration.Modes
{
    /// <summary>
    /// All listed types will be invalid.
    /// </summary>
    /// <typeparam name="TBase">Type which can be filtered.</typeparam>
    public class BlacklistTypeRule<TBase> : ListTypeRule<BlacklistTypeRule<TBase>, TBase>
    {
        /// <inheritdoc />
        protected override bool IsQualifiedByPredicate(Type type)
        {
            return StoredTypes.Contains(type) == false;
        }
    }
}
