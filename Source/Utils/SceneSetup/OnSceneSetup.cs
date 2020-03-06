namespace Innoactive.Creator.Utils
{
    /// <summary>
    /// This interface is supposed to be implemented by classes which will be called to setup the scene.
    /// Can be used to e.g. setup training classes or interaction frameworks.
    /// </summary>
    /// <remarks>
    /// See <see cref="TrainingSceneSetup"/> as a reference.
    /// </remarks>
    public abstract class OnSceneSetup
    {
        /// <summary>
        /// Setup the scene with necessary objects and/or logic.
        /// </summary>
        public abstract void Setup();
    }
}
