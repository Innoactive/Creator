namespace VPG.Creator.Core
{
    /// <summary>
    /// Base class for implementing post processing after a <see cref="IEntity"/> creation.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IEntity"/>.</typeparam>
    public abstract class EntityPostProcessing<T> where T : IEntity
    {
        /// <summary>
        /// Executes post processing.
        /// </summary>
        /// <param name="entity"><see cref="IEntity"/> reference for post processing.</param>
        public abstract void Execute(T entity);
    }
}
