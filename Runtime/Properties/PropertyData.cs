using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    /// <summary>
    /// Contains a target <see cref="LockableProperty"/> and additional information which define how the property is handled.
    /// </summary>
    public class PropertyData
    {
        /// <summary>
        /// Target lockable property.
        /// </summary>
        public readonly TrainingSceneObjectProperty Property;

        public PropertyData(TrainingSceneObjectProperty property)
        {
            Property = property;
        }

        protected bool Equals(PropertyData other)
        {
            return Equals(Property, other.Property);
        }

        ///  <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertyData) obj);
        }

        ///  <inheritdoc/>
        public override int GetHashCode()
        {
            return (Property != null ? Property.GetHashCode() : 0);
        }
    }
}
