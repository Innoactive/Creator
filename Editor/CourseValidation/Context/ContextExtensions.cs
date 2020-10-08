namespace Innoactive.CreatorEditor.CourseValidation
{
    internal static class ContextExtensions
    {
        /// <summary>
        /// Looks up if any parent of child is the given context.
        /// </summary>
        public static bool IsChildOf(this IContext child, IContext context)
        {
            IContext parent = child;
            while ((parent = parent.Parent) != null)
            {
                if (parent.Equals(context))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if any parent is a specific context type.
        /// </summary>
        public static bool ContainsContext<T>(this IContext child) where T : IContext
        {
            if (child.GetType() == typeof(T))
            {
                return true;
            }

            IContext parent = child;
            while ((parent = parent.Parent) != null)
            {
                if (parent.GetType() == typeof(T))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
