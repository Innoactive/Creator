using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Training.Utils
{
    public static class TrainingReferenceUtils
    {
        public static string GetNameFrom(ISceneObjectProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.SceneObject == null)
            {
                return null;
            }

            return property.SceneObject.UniqueName;
        }

        public static string GetNameFrom(ISceneObject sceneObject)
        {
            if (sceneObject == null)
            {
                return null;
            }

            return sceneObject.UniqueName;
        }
    }
}
