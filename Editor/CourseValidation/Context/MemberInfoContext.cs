using System.Reflection;
using Innoactive.Creator.Core.Attributes;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class MemberInfoContext : IContext
    {
        /// <inheritdoc/>
        public bool IsSelectable { get; } = false;

        /// <inheritdoc/>
        public IContext Parent { get; }

        internal MemberInfo MemberInfo { get; }

        public MemberInfoContext(MemberInfo info, IContext parent)
        {
            MemberInfo = info;
            Parent = parent;
        }

        /// <inheritdoc/>
        public void Select()
        {
            throw new System.NotImplementedException();
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
    }
}
