namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Context is used to indicate the position in the course structure.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Parent context, can be null.
        /// </summary>
        IContext Parent { get; }

        /// <summary>
        /// Produces a readable string which allows us to find the context in editor.
        /// </summary>
        string ToString();
    }
}
