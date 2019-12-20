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
        /// Get grab step builder.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="objectsToGrab">list of the objects that have to be grabbed before training chapter continues.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Grab(string name, params GrabbableProperty[] objectsToGrab)
        {
            return Grab(name, objectsToGrab.Select(TrainingReferenceUtils.GetNameFrom).ToArray());
        }

        /// <summary>
        /// Get grab step builder.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="objectsToGrab">list of the objects that have to be grabbed before training chapter continues.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Grab(string name, params string[] objectsToGrab)
        {
            BasicStepBuilder builder = new BasicStepBuilder(name);

            foreach (string objectToGrab in objectsToGrab)
            {
                builder.AddCondition(new GrabbedCondition(objectToGrab));
            }

            return builder;
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

        /// <summary>
        /// Get builder for a step during which user has to put objects into a snap zone.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="snapZone">Snap zone in which user should put objects.</param>
        /// <param name="objectsToPut">List of objects to put into collider.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder PutIntoSnapZone(string name, SnapZoneProperty snapZone, params SnappableProperty[] objectsToPut)
        {
            BasicStepBuilder builder = new BasicStepBuilder(name);

            foreach (SnappableProperty objectToPut in objectsToPut)
            {
                builder.AddCondition(new SnappedCondition(objectToPut, snapZone));
            }

            return builder;
        }

        /// <summary>
        /// Get builder for a step during which user has to put objects into a snap zone.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="snapZone">Snap zone in which user should put objects.</param>
        /// <param name="objectsToPut">List of objects to put into collider.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder PutIntoSnapZone(string name, string snapZone, params string[] objectsToPut)
        {
            return PutIntoSnapZone(name, GetFromRegistry(snapZone).GetProperty<SnapZoneProperty>(), objectsToPut.Select(GetFromRegistry).Select(t => t.GetProperty<SnappableProperty>()).ToArray());
        }

        /// <summary>
        /// Get builder for a step during which user has to activate some objects.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="objectsToUse">List of objects to use.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Use(string name, params UsableProperty[] objectsToUse)
        {
            BasicStepBuilder builder = new BasicStepBuilder(name);

            foreach (UsableProperty objectToUse in objectsToUse)
            {
                builder.AddCondition(new UsedCondition(objectToUse));
            }

            return builder;
        }

        /// <summary>
        /// Get builder for a step during which user has to activate some objects.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="objectsToUse">List of objects to use.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Use(string name, params string[] objectsToUse)
        {
            return Use(name, objectsToUse.Select(GetFromRegistry).Select(t => t.GetProperty<UsableProperty>()).ToArray());
        }

        /// <summary>
        /// Get builder for a step during which user has to touch some objects.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="objectsToTouch">List of objects to touch.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Touch(string name, params ISceneObject[] objectsToTouch)
        {
            return Touch(name, objectsToTouch.Select(TrainingReferenceUtils.GetNameFrom).ToArray());
        }

        /// <summary>
        /// Get builder for a step during which user has to touch some objects.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        /// <param name="objectsToTouch">List of objects to touch.</param>
        /// <returns>Configured builder.</returns>
        public static BasicStepBuilder Touch(string name, params string[] objectsToTouch)
        {
            BasicStepBuilder builder = new BasicStepBuilder(name);

            foreach (string objectToTouch in objectsToTouch)
            {
                builder.AddCondition(new TouchedCondition(objectToTouch));
            }

            return builder;
        }
    }
}
