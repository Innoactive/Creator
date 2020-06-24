using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    public class LockablePropertyReference
    {
        public readonly LockableProperty Property;
        
        public readonly bool EndStepLocked;

        public LockablePropertyReference(LockableProperty property, bool endStepLocked = true)
        {
            EndStepLocked = endStepLocked;
            Property = property;
        }


        protected bool Equals(LockablePropertyReference other)
        {
            return Equals(Property, other.Property);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LockablePropertyReference) obj);
        }

        public override int GetHashCode()
        {
            return (Property != null ? Property.GetHashCode() : 0);
        }
    }
}