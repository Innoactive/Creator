using System.Reflection;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Attributes;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class MemberInfoContext : IContext
    {
        /// <inheritdoc/>
        public IContext Parent { get; }

        /// <summary>
        /// Parent IData object.
        /// </summary>
        public IData ParentData { get; }

        /// <summary>
        /// MemberInfo of the field/property pointed out by this context.
        /// </summary>
        public MemberInfo MemberInfo { get; }

        public MemberInfoContext(MemberInfo info, IData parentData, IContext parent)
        {
            MemberInfo = info;
            ParentData = parentData;
            Parent = parent;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [{GetMemberInfoName()}]";
            }
            return $"[{GetMemberInfoName()}]";
        }

        private string GetMemberInfoName()
        {
            DisplayNameAttribute nameAttribute = MemberInfo.GetCustomAttribute<DisplayNameAttribute>();
            return nameAttribute != null ? nameAttribute.Name : MemberInfo.Name;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MemberInfoContext) obj);
        }

        protected bool Equals(MemberInfoContext other)
        {
            return Equals(MemberInfo, other.MemberInfo);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (MemberInfo != null ? MemberInfo.GetHashCode() : 0);
        }
    }
}
