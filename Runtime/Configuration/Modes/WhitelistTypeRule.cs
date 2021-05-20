using System;

namespace VPG.Core.Configuration.Modes
{
    /// <summary>
    /// All listed types will be valid.
    /// </summary>
    /// <typeparam name="TBase">Type which can be filtered.</typeparam>
    public class WhitelistTypeRule<TBase> : ListTypeRule<WhitelistTypeRule<TBase>, TBase>
    {
        /// <inheritdoc />
        protected override bool IsQualifiedByPredicate(Type type)
        {
            return StoredTypes.Contains(type);
        }
    }
}
