namespace Innoactive.CreatorEditor.CourseValidation
{
    public static class ContextExtensions
    {
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
