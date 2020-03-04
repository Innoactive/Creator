using System.Linq;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Training.Utils.Builders
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

        /// <summary>
        /// Get builder for a step where trainee has to enter collider.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="targetAreaCollider">Target collider for a trainee to enter.</param>
        /// <param name="triggerDelay">How long trainee should stay inside the collider to continue.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder TraineeInArea(string name, string targetAreaCollider, float triggerDelay = 0f)
        {
            return TraineeInArea(name, GetFromRegistry(targetAreaCollider), triggerDelay);
        }

        /// <summary>
        /// Get builder for a step where trainee has to enter collider.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="targetAreaCollider">Target collider for a trainee to enter.</param>
        /// <param name="triggerDelay">How long trainee should stay inside the collider to continue.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder TraineeInArea(string name, ISceneObject targetAreaCollider, float triggerDelay = 0f)
        {
            return PutIntoCollider(name, targetAreaCollider, triggerDelay, RuntimeConfigurator.Configuration.Trainee);
        }

        /// <summary>
        /// Get builder for a step during which user has to put given objects into given collider.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="targetCollider">Collider in which user should put objects.</param>
        /// <param name="triggerDelay">How long an object should be inside the collider to be registered.</param>
        /// <param name="objectsToPut">List of objects to put into collider.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder PutIntoCollider(string name, ISceneObject targetCollider, float triggerDelay = 0f, params ISceneObject[] objectsToPut)
        {
            return PutIntoCollider(name, TrainingReferenceUtils.GetNameFrom(targetCollider), triggerDelay, objectsToPut.Select(TrainingReferenceUtils.GetNameFrom).ToArray());
        }

        /// <summary>
        /// Get builder for a step during which user has to put given objects into given collider.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="targetCollider">Collider in which user should put objects.</param>
        /// <param name="triggerDelay">How long an object should be inside the collider to be registered.</param>
        /// <param name="objectsToPut">List of objects to put into collider.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder PutIntoCollider(string name, string targetCollider, float triggerDelay = 0f, params string[] objectsToPut)
        {
            BasicStepBuilder builder = new BasicStepBuilder(name);

            foreach (string objectToPut in objectsToPut)
            {
                builder.AddCondition(new ObjectInColliderCondition(targetCollider, objectToPut, 0));
            }

            return builder;
        }
    }
}
