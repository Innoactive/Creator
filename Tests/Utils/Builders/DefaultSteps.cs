using System.Linq;
using VPG.Core.Conditions;
using VPG.Core.Configuration;
using VPG.Core.SceneObjects;

namespace VPG.Tests.Builder
{
    /// <summary>
    /// Static class to provide fast access to predefined builders.
    /// </summary>
    public static class DefaultSteps
    {
        /// <summary>
        /// Gets the <see cref="ISceneObject"/> with given <paramref name="name"/> from the registry.
        /// </summary>
        /// <param name="name">Name of scene object.</param>
        /// <returns><see cref="ISceneObject"/> with given name.</returns>
        private static ISceneObject GetFromRegistry(string name)
        {
            return RuntimeConfigurator.Configuration.SceneObjectRegistry[name];
        }

        /// <summary>
        /// Get intro step builder.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <returns>Basic step builder with configured name.</returns>
        public static BasicStepBuilder Intro(string name)
        {
            return new BasicStepBuilder(name);
        }
    }
}
