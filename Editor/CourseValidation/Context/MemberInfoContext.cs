using System.Reflection;
using Innoactive.Creator.Core;
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

        public IData ParentData { get; }

        internal MemberInfo MemberInfo { get; }

        public MemberInfoContext(MemberInfo info, IData parentData, IContext parent)
        {
            MemberInfo = info;
            ParentData = parentData;
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is MemberInfoContext context)
            {
                if (Parent != null && Parent.Equals(context.Parent) == false)
                {
                    return false;
                }

                return MemberInfo.Equals(context.MemberInfo);
            }

            return false;
        }
    }
}
