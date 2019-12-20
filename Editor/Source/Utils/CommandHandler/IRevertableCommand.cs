namespace Innoactive.Hub.Training.Editors.Utils.Undo
{
    /// <summary>
    /// An interface for a method object,
    /// </summary>
    public interface IRevertableCommand
    {
        /// <summary>
        /// Perform some revertable action.
        /// </summary>
        void Do();

        /// <summary>
        /// Revert the changes done during by <see cref="Do"/> method.
        /// </summary>
        void Undo();
    }
}
