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
        /// If this is true, you can call Select on this object.
        /// </summary>
        bool IsSelectable { get; }

        /// <summary>
        /// Will select & focus this context in editor.
        /// </summary>
        void Select();

        /// <summary>
        /// Produces a readable string which allows us to find the context in editor.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
