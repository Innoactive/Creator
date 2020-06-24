using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    public class LockablePropertyData
    {
        public readonly LockableProperty Property;
        
        public readonly bool EndStepLocked;

        public LockablePropertyData(LockableProperty property) : this(property, property.EndStepLocked) { }
        
        public LockablePropertyData(LockableProperty property, bool endStepLocked)
        {
            EndStepLocked = endStepLocked;
            Property = property;
        }

        protected bool Equals(LockablePropertyData other)
        {
            return Equals(Property, other.Property);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LockablePropertyData) obj);
        }

        public override int GetHashCode()
        {
            return (Property != null ? Property.GetHashCode() : 0);
        }
    }
}